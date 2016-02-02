using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class TweenBaseInspector : Editor
	{
		TweenBase _Target;
		void OnEnable()
		{
			_Target = target as TweenBase;
		}
		protected void DrawBase()
		{
			SerializedProperty typeProperty = serializedObject.FindProperty("_Type");

            EditorGUILayout.PropertyField( typeProperty );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Duration" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Curve" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_UseRealtime" ) );
			
			TweenBase.Type type = (TweenBase.Type)typeProperty.enumValueIndex;
			if (type != TweenBase.Type.Once)
			{
				SerializedProperty repeatUntilTransition = serializedObject.FindProperty("_RepeatUntilTransition");
                EditorGUILayout.PropertyField(repeatUntilTransition);
				repeatUntilTransition.intValue = Mathf.Max(repeatUntilTransition.intValue, 1);

				if (Application.isPlaying && _Target.stateMachine.currentState == _Target.state)
				{
					Rect r = EditorGUILayout.BeginVertical();
					EditorGUI.ProgressBar(r, (float)_Target.repeatCount / (float)repeatUntilTransition.intValue, _Target.repeatCount.ToString() );
					GUILayout.Space(16);
					EditorGUILayout.EndVertical();
				}
            }
		}
	}
}