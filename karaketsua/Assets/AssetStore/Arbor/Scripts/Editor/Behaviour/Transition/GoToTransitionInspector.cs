using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.GoToTransition))]
	public class GoToTransitionInspector : Editor
	{
		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}
}