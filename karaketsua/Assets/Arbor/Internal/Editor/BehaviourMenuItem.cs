using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class BehaviourMenuItem : Attribute
	{
		public Type type;
		public string menuItem;
		public bool validate;
		public int priority;

		public BehaviourMenuItem(Type type,string itemName)
		{
			this.type = type;
			this.menuItem = itemName;
			this.validate = false;
			this.priority = 1000;
		}

		public BehaviourMenuItem(Type type, string itemName,bool isValidateFunction)
		{
			this.type = type;
			this.menuItem = itemName;
			this.validate = isValidateFunction;
			this.priority = 1000;
		}

		public BehaviourMenuItem(Type type, string itemName, bool isValidateFunction, int priority)
		{
			this.type = type;
			this.menuItem = itemName;
			this.validate = isValidateFunction;
			this.priority = priority;
        }
	}
}
