using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class ParameterContainerCreator
	{
		[MenuItem("GameObject/Arbor/ParameterContainer", false, 10)]
		public static void CreateParameterContainer(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("ParameterContainer", typeof(ParameterContainer));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create ParameterContainer");
			Selection.activeGameObject = gameObject;
		}
	}
}