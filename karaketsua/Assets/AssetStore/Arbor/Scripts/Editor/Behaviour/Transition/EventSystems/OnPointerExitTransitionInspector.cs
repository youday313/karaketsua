using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(OnPointerExitTransition))]
	public class OnPointerExitTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}
}