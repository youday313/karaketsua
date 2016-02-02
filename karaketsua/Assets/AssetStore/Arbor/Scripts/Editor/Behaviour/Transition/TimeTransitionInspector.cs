using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(TimeTransition))]
	public class TimeTransitionInspector : Editor
	{
		TimeTransition _Target;

		void OnEnable()
		{
			_Target = target as TimeTransition;
		}
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			SerializedProperty secondsProperty = serializedObject.FindProperty("_Seconds");

			EditorGUILayout.PropertyField(secondsProperty);

			if (Application.isPlaying && _Target.stateMachine.currentState == _Target.state && secondsProperty.floatValue > 0.0f )
			{
				Rect r = EditorGUILayout.BeginVertical();
				EditorGUI.ProgressBar(r, _Target.elapsedTime / secondsProperty.floatValue, _Target.elapsedTime.ToString("0.00") );
				GUILayout.Space(16);
				EditorGUILayout.EndVertical();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}