using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class ArborFSMCreator
	{
		[MenuItem("GameObject/Arbor/ArborFSM", false,10)]
		public static void CreateArborFSM(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("ArborFSM", typeof(ArborFSM));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create ArborFSM");
			Selection.activeGameObject = gameObject;
		}
	}
}