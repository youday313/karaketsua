using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(GameObjectParameterReference))]
	public class GameObjectParameterReferencePropertyDrawer : ParameterReferencePropertyDrawer
	{
		protected override bool CheckType(Parameter.Type type)
		{
			return type == Parameter.Type.GameObject;
		}
	}
}
