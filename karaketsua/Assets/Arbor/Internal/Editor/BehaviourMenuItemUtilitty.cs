using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	[InitializeOnLoad]
	public class BehaviourMenuItemUtilitty : MonoBehaviour
	{
		public class Element
		{
			public BehaviourMenuItem menuItem;
			public MethodInfo method;
			public int index;
		}

		static List<Element> _Elements = new List<Element>();

		public static Element[] elements
		{
			get
			{
				return _Elements.ToArray();
			}
		}

		static BehaviourMenuItemUtilitty()
		{
			foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (System.Type type in assembly.GetTypes())
				{
					MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int index = 0; index < methods.Length; ++index)
					{
						MethodInfo method = methods[index];
						foreach (BehaviourMenuItem menuItem in method.GetCustomAttributes(typeof(BehaviourMenuItem), false))
						{
							Element element = new Element();
							element.menuItem = menuItem;
							element.method = method;
							element.index = index;
							_Elements.Add(element);
						}
					}
				}
			}
		}
	}
}
