using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Arbor;

namespace ArborEditor
{
	[InitializeOnLoad]
	public class BehaviourMenuWindow : EditorWindow
	{
		private class Styles
		{
			public static GUIStyle header;
			public static GUIStyle componentButton;
			public static GUIStyle groupButton;
			public static GUIStyle background;
			public static GUIStyle previewBackground;
			public static GUIStyle previewHeader;
			public static GUIStyle previewText;
			public static GUIStyle rightArrow;
			public static GUIStyle leftArrow;

			static Styles()
			{
				header = new GUIStyle((GUIStyle)"IN BigTitle");
                componentButton = new GUIStyle((GUIStyle)"PR Label");
				background = (GUIStyle)"grey_border";
				previewBackground = (GUIStyle)"PopupCurveSwatchBackground";
				previewHeader = new GUIStyle(EditorStyles.label);
				previewText = new GUIStyle(EditorStyles.wordWrappedLabel);
				rightArrow = (GUIStyle)"AC RightArrow";
				leftArrow = (GUIStyle)"AC LeftArrow";
				header.font = EditorStyles.boldLabel.font;
				componentButton.alignment = TextAnchor.MiddleLeft;
				componentButton.padding.left -= 15;
				componentButton.fixedHeight = 20f;
				groupButton = new GUIStyle(componentButton);
				groupButton.padding.left += 17;
				previewText.padding.left += 3;
				previewText.padding.right += 3;
				++previewHeader.padding.left;
				previewHeader.padding.right += 3;
				previewHeader.padding.top += 3;
				previewHeader.padding.bottom += 2;
			}
		}

		private static BehaviourMenuWindow _Instance;

		public static BehaviourMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<BehaviourMenuWindow>();
				}
				return _Instance;
			}
		}

		private class Element : System.IComparable
		{
			public int level;
			public GUIContent content;

			public string name
			{
				get
				{
					return content.text;
				}
			}

			public int CompareTo(object obj)
			{
				return name.CompareTo((obj as Element).name);
			}
		}

		private class BehaviourElement : Element
		{
			public System.Type classType;
			
			public BehaviourElement(int level, string name, System.Type classType,Texture icon)
			{
				this.level = level;
				this.classType = classType;

				this.content = new GUIContent(name,icon);
			}
		}

		[System.Serializable]
		private class GroupElement : Element
		{
			public Vector2 scroll;
			public int selectedIndex;

			public GroupElement(int level, string name)
			{
				this.level = level;
				this.content = new GUIContent(name);
			}
		}

		private static bool s_DirtyList;

		private Element[] _Tree;
		private Element[] _SearchResultTree;

		private List<GroupElement> _Stack = new List<GroupElement>();
		private float _Anim = 1f;
		private int _AnimTarget = 1;
		private long _LastTime;
		private bool _ScrollToSelected;
		private string _Search = string.Empty;
		private State _State;

		private bool hasSearch
		{
			get
			{
				return !string.IsNullOrEmpty(_Search);
			}
		}

		private GroupElement activeParent
		{
			get
			{
				return _Stack[_Stack.Count - 2 + _AnimTarget];
			}
		}

		private Element[] activeTree
		{
			get
			{
				if (hasSearch)
				{
					return _SearchResultTree;
				}
				return _Tree;
			}
		}

		private Element activeElement
		{
			get
			{
				if (activeTree == null)
				{
					return null;
				}
				List<Element> children = GetChildren(activeTree, activeParent );
				if (children.Count == 0)
				{
					return null;
				}
				return children[activeParent.selectedIndex];
			}
		}

		static BehaviourMenuWindow()
		{
			s_DirtyList = true;
		}

		private List<Element> GetChildren(Element[] tree, Element parent)
		{
			List<Element> list = new List<Element>();
			int num = -1;
			int index;
			for (index = 0; index < tree.Length; ++index)
			{
				if (tree[index] == parent)
				{
					num = parent.level + 1;
					++index;
					break;
				}
			}
			if (num == -1)
				return list;
			for (; index < tree.Length; ++index)
			{
				Element element = tree[index];
				if (element.level >= num)
				{
					if (element.level <= num || hasSearch)
						list.Add(element);
				}
				else
					break;
			}
			return list;
		}

		private void CreateBehaviourTree()
		{
			List<string> list1 = new List<string>();
			List<Element> list2 = new List<Element>();

			list2.Add(new GroupElement(0, "Behaviours"));

			foreach (BehaviourMenuUtility.BehaviourMenuItem item in BehaviourMenuUtility.items)
			{
				string str = item.menuName;
				string[] strArray = str.Split(new char[] { '/' });

				while (strArray.Length - 1 < list1.Count)
					list1.RemoveAt(list1.Count - 1);
				while (list1.Count > 0 && strArray[list1.Count - 1] != list1[list1.Count - 1])
					list1.RemoveAt(list1.Count - 1);
				while (strArray.Length - 1 > list1.Count)
				{
					list2.Add(new GroupElement(list1.Count + 1, strArray[list1.Count]));
					list1.Add(strArray[list1.Count]);
				}
				list2.Add(new BehaviourElement(list1.Count + 1, strArray[strArray.Length - 1], item.classType, item.icon));
			}

			_Tree = list2.ToArray();
			if (_Stack.Count == 0)
			{
				_Stack.Add(_Tree[0] as GroupElement);
			}
			else
			{
				GroupElement groupElement = _Tree[0] as GroupElement;
				int level = 0;
				while (true)
				{
					GroupElement groupElement2 = _Stack[level];
					_Stack[level] = groupElement;
					_Stack[level].selectedIndex = groupElement2.selectedIndex;
					_Stack[level].scroll = groupElement2.scroll;
					level++;
					if (level == _Stack.Count)
					{
						break;
					}
					List<Element> children = GetChildren(activeTree, groupElement);
					Element element = children.FirstOrDefault((c) => c.name == _Stack[level].name);
					if (element != null && element is GroupElement)
					{
						groupElement = (element as GroupElement);
					}
					else
					{
						while (_Stack.Count > level)
						{
							_Stack.RemoveAt(level);
						}
					}
				}
			}
			s_DirtyList = false;
			RebuildSearch();
		}

		private void RebuildSearch()
		{
			if ( !hasSearch )
			{
				_SearchResultTree = null;
				if (_Stack[_Stack.Count - 1].name == "Search")
				{
					_Stack.Clear();
					_Stack.Add(_Tree[0] as GroupElement);
				}
				_AnimTarget = 1;
				_LastTime = System.DateTime.Now.Ticks;
			}
			else
			{
				string str1 = _Search.ToLower();
				string[] strArray = str1.Split( new char[] {' '} );

				List<Element> list1 = new List<Element>();
				List<Element> list2 = new List<Element>();
				foreach (Element element in _Tree)
				{
					if (element is BehaviourElement)
					{
						string str2 = element.name.ToLower().Replace(" ", string.Empty);
						bool flag1 = true;
						bool flag2 = false;
						for (int index2 = 0; index2 < strArray.Length; ++index2)
						{
							string str3 = strArray[index2];
							if (str2.Contains(str3) )
							{
								if (index2 == 0 && str2.StartsWith(str3))
								{
									flag2 = true;
								}
							}
							else
							{
								flag1 = false;
								break;
							}
						}
						if (flag1)
						{
							if (flag2)
								list1.Add(element);
							else
								list2.Add(element);
						}
					}
				}
				list1.Sort();
				list2.Sort();
				List<Element> list3 = new List<Element>();
				list3.Add((Element)new GroupElement(0, "Search"));
				list3.AddRange(list1);
				list3.AddRange(list2);
				_SearchResultTree = list3.ToArray();
				_Stack.Clear();
				_Stack.Add(_SearchResultTree[0] as GroupElement);
				if( GetChildren(activeTree,activeParent).Count >= 1 )
				{
					activeParent.selectedIndex = 0;
				}
				else
				{
					activeParent.selectedIndex = -1;
				}
			}
		}

		private GroupElement GetElementRelative(int rel)
		{
			int index = _Stack.Count + rel - 1;
			if (index < 0)
			{
				return null;
			}
			return _Stack[index];
		}

		private void GoToParent()
		{
			if (_Stack.Count <= 1)
			{
				return;
			}
			_AnimTarget = 0;
			_LastTime = System.DateTime.Now.Ticks;
		}

		private void ListGUI(Element[] tree, float anim, GroupElement parent, GroupElement grandParent)
		{
			anim = Mathf.Floor(anim) + Mathf.SmoothStep(0.0f, 1f, Mathf.Repeat(anim, 1f));
			Rect position1 = this.position;
			position1.x = (float)((double)this.position.width * (1.0 - (double)anim) + 1.0);
			position1.y = 40f;
			position1.height -= 40f;
			position1.width -= 2f;
			GUILayout.BeginArea(position1);
			Rect rect = GUILayoutUtility.GetRect(10f, 25f);
			string name = parent.name;
			GUI.Label(rect, name, Styles.header);
			if (grandParent != null)
			{
				Rect position2 = new Rect(rect.x + 4f, rect.y + 7f, 13f, 13f);
				if (Event.current.type == EventType.Repaint)
				{
					Styles.leftArrow.Draw(position2, false, false, false, false);
				}
				if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
				{
					this.GoToParent();
					Event.current.Use();
				}
			}
			ListGUI(tree, parent);
			GUILayout.EndArea();
		}

		private void GoToChild(Element e, bool addIfComponent)
		{
			if (e is BehaviourElement)
			{
				if (!addIfComponent)
					return;

				BehaviourElement behaviourElement = e as BehaviourElement;

				Undo.RecordObject(_State.stateMachine, "Add State Behaviour");

				_State.AddBehaviour(behaviourElement.classType);

				EditorUtility.SetDirty(_State.stateMachine);

				Close();
			}
			else
			{
				if ( hasSearch )
					return;
				_LastTime = System.DateTime.Now.Ticks;
				if (_AnimTarget == 0)
				{
					_AnimTarget = 1;
				}
				else
				{
					if (_Anim != 1.0f)
						return;
					_Anim = 0.0f;
					_Stack.Add(e as GroupElement);
				}
			}
		}

		private void ListGUI(Element[] tree, GroupElement parent)
		{
			parent.scroll = GUILayout.BeginScrollView(parent.scroll);
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			List<Element> children = GetChildren(tree, parent);
			Rect rect1 = new Rect();
			for (int index1 = 0; index1 < children.Count; ++index1)
			{
				Element e = children[index1];
				Rect rect2 = GUILayoutUtility.GetRect(16.0f, 20.0f, GUILayout.ExpandWidth(true));
				if ((Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown) && (parent.selectedIndex != index1 && rect2.Contains(Event.current.mousePosition)))
				{
					parent.selectedIndex = index1;
					this.Repaint();
				}
				bool flag = false;
				if (index1 == parent.selectedIndex)
				{
					flag = true;
					rect1 = rect2;
				}
				if (Event.current.type == EventType.Repaint)
				{
					(!(e is BehaviourElement) ? Styles.groupButton : Styles.componentButton).Draw(rect2, e.content, false, false, flag, flag);
					if (!(e is BehaviourElement))
					{
						Rect position = new Rect((float)((double)rect2.x + (double)rect2.width - 13.0), rect2.y + 4f, 13f, 13f);
						Styles.rightArrow.Draw(position, false, false, false, false);
					}
				}
				if (Event.current.type == EventType.MouseDown && rect2.Contains(Event.current.mousePosition))
				{
					Event.current.Use();
					parent.selectedIndex = index1;
					GoToChild(e, true);
				}
			}
			EditorGUIUtility.SetIconSize(Vector2.zero);
			GUILayout.EndScrollView();
			if (!_ScrollToSelected || Event.current.type != EventType.Repaint)
				return;
			_ScrollToSelected = false;
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if (rect1.yMax - lastRect.height > parent.scroll.y)
			{
				parent.scroll.y = rect1.yMax - lastRect.height;
				Repaint();
			}
			if (rect1.y >= parent.scroll.y)
			{
				return;
			}
			parent.scroll.y = rect1.y;
			Repaint();
		}

		private void OnEnable()
		{
			_Search = EditorPrefs.GetString("BehaviourSearchString", string.Empty);
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector2 = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector2.x;
			guiRect.y = vector2.y;
			return guiRect;
		}

		public void Init(State state, Rect buttonRect)
		{
			buttonRect = GUIToScreenRect(buttonRect);
			CreateBehaviourTree();
			ShowAsDropDown(buttonRect, new Vector2(300f, 320f));
			Focus();

			_State = state;
		}

		void DrawBackground()
		{
			if (Event.current.type == EventType.Repaint)
			{
				Styles.background.Draw(position, false, false, false, false);
            }
		}

		void SearchGUI()
		{
			Rect rect = GUILayoutUtility.GetRect(10f, 20f);
			rect.x += 8f;
			rect.width -= 16f;
			GUI.SetNextControlName("ArborBehaviourSearch");
			string str = EditorGUITools.SearchField(rect, _Search);
			if (str != _Search)
			{
				_Search = str;
				EditorPrefs.SetString("ArborBehaviourSearchString", _Search);
				RebuildSearch();
			}
		}

		private void HandleKeyboard()
		{
			Event current = Event.current;
			if (current.type != EventType.KeyDown)
			{
				return;
			}
			
			if (current.keyCode == KeyCode.DownArrow)
			{
				++activeParent.selectedIndex;
				activeParent.selectedIndex = Mathf.Min(activeParent.selectedIndex, GetChildren(activeTree, activeParent).Count - 1);
				_ScrollToSelected = true;
				current.Use();
			}
			if (current.keyCode == KeyCode.UpArrow)
			{
				--activeParent.selectedIndex;
				activeParent.selectedIndex = Mathf.Max(activeParent.selectedIndex, 0);
				_ScrollToSelected = true;
				current.Use();
			}
			if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
			{
				GoToChild(activeElement, true);
				current.Use();
			}
			if (hasSearch)
			{
				return;
			}
			if (current.keyCode == KeyCode.LeftArrow || current.keyCode == KeyCode.Backspace)
			{
				GoToParent();
				current.Use();
			}
			if (current.keyCode == KeyCode.RightArrow)
			{
				GoToChild(activeElement, false);
				current.Use();
			}
			if (current.keyCode != KeyCode.Escape)
			{
				return;
			}
			Close();
			current.Use();
		}

		void OnGUI()
		{
			DrawBackground();

			if (s_DirtyList)
			{
				CreateBehaviourTree();
			}

			HandleKeyboard();

			SearchGUI();

			ListGUI(activeTree, _Anim, GetElementRelative(0), GetElementRelative(-1) );
			if (_Anim < 1.0)
			{
				ListGUI(activeTree, _Anim + 1f, GetElementRelative(-1), GetElementRelative(-2));
			}

			if( _Anim == _AnimTarget || Event.current.type != EventType.Repaint)
				return;
			long ticks = System.DateTime.Now.Ticks;
			float num = (float)(ticks - _LastTime) / 1E+07f;
			_LastTime = ticks;
			_Anim = Mathf.MoveTowards(_Anim, _AnimTarget, num * 4f);
			if (_AnimTarget == 0 && _Anim == 0.0f)
			{
				_Anim = 1f;
				_AnimTarget = 1;
				_Stack.RemoveAt(_Stack.Count - 1);
			}
			Repaint();
		}
	}
}