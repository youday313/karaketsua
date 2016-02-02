using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(GoToTransition))]
	public class GoToTransitionInspector : Editor
	{
		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}
}