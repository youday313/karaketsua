using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[InitializeOnLoad]
	internal class BehaviourMenuUtility
	{
		static BehaviourMenuUtility()
		{
			UpdateBehaviourMenuItems();
		}

		public class BehaviourMenuItem
		{
			public string menuName;
			public Texture icon;
			public System.Type classType;
		};
		
		private static List<BehaviourMenuItem> _BehaviourMenuItems = new List<BehaviourMenuItem>();

		public static BehaviourMenuItem[] items
		{
			get
			{
				return _BehaviourMenuItems.ToArray();
			}
		}

		private static void UpdateBehaviourMenuItems()
		{
			_BehaviourMenuItems.Clear();
			
			foreach( MonoScript script in MonoImporter.GetAllRuntimeMonoScripts() )
			{
				if( script != null && script.hideFlags == 0 )
				{
					System.Type classType = script.GetClass();
					if( classType != null )
					{
						if( classType.IsSubclassOf( typeof(StateBehaviour) ) )
						{
							object[] hideAttributes = classType.GetCustomAttributes( typeof(HideBehaviour),false );
							
							if( hideAttributes != null && hideAttributes.Length > 0 )
							{
								continue;
							}

							BehaviourMenuItem menuItem = new BehaviourMenuItem();
							menuItem.classType = classType;
							menuItem.icon = AssetDatabase.GetCachedIcon( AssetDatabase.GetAssetPath(script) );

							menuItem.menuName = "Scripts/"+classType.Name;
							
							object[] attributes = classType.GetCustomAttributes( typeof(AddBehaviourMenu),false );
							
							if( attributes != null && attributes.Length > 0 )
							{
								AddBehaviourMenu behaviourMenu = attributes[0] as AddBehaviourMenu;
								
								if( behaviourMenu != null )
								{
									menuItem.menuName = behaviourMenu.menuName;
								}
							}
							
							_BehaviourMenuItems.Add( menuItem );
						}
					}
				}
			}
			
			_BehaviourMenuItems.Sort( (a,b)=>{ return a.menuName.CompareTo( b.menuName ); } );
		}

		private static void AddBehaviourContextMenu( object obj )
		{
			KeyValuePair<State,System.Type> pair = (KeyValuePair<State,System.Type>)obj;
			
			State state = pair.Key;
			System.Type classType = pair.Value;
			
			Undo.RecordObject( state.stateMachine,"Add State Behaviour" );
			
			state.AddBehaviour( classType );
			
			EditorUtility.SetDirty( state.stateMachine );
		}

		public static void AddMenu( State state,GenericMenu menu )
		{
			foreach( BehaviourMenuItem menuItem in _BehaviourMenuItems )
			{
				menu.AddItem( new GUIContent( Localization.GetWord("Add Behaviour") +"/" + menuItem.menuName ),false,AddBehaviourContextMenu,new KeyValuePair<State,System.Type>(state,menuItem.classType) );
			}
		}
	}
}