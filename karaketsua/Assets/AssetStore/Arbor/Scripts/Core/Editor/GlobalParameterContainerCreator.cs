using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class GlobalParameterContainerCreator
	{
		[MenuItem("GameObject/Arbor/GlobalParameterContainer", false, 10)]
		public static void CreateGlobalParameterContainer(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("GlobalParameterContainer", typeof(GlobalParameterContainer));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create GlobalParameterContainer");
			Selection.activeGameObject = gameObject;
		}
	}
}