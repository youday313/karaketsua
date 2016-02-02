using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class AgentCreator
	{
		[MenuItem("GameObject/Arbor/Agent", false, 10)]
		public static void CreateAgent(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("Agent", typeof(NavMeshAgent),typeof(AgentController));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create Agent");
			Selection.activeGameObject = gameObject;
		}
	}
}