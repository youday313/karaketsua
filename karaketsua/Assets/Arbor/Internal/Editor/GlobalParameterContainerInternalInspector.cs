using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class GlobalParameterContainerInternalInspector : Editor
	{
		private GlobalParameterContainerInternal _Target;

		void OnEnable()
		{
			_Target = target as GlobalParameterContainerInternal;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(Application.isPlaying);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Prefab"));

			if (Application.isPlaying)
			{
				EditorGUILayout.ObjectField("Instance", _Target.instance,typeof(ParameterContainerInternal),true );
            }

			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();

		}
	}
}
