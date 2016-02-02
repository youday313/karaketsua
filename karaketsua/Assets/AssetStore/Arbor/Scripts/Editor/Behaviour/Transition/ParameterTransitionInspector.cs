using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ParameterTransition))]
	public class ParameterTransitionInspector : Editor
	{
		private void CondisionGUI(ParameterContainer container, SerializedProperty referenceProperty, SerializedProperty condisionProperty)
		{
			SerializedProperty idProperty = referenceProperty.FindPropertyRelative("id");

			Parameter parameter = container.GetParam(idProperty.intValue);
			
			if (parameter != null)
			{
				SerializedProperty typeProperty = condisionProperty.FindPropertyRelative("_Type");

				switch (parameter.type)
				{
					case Parameter.Type.Int:
						{
							EditorGUILayout.PropertyField(typeProperty);

							EditorGUILayout.PropertyField(condisionProperty.FindPropertyRelative("_IntValue"), new GUIContent("Int Value"));
						}
						break;
					case Parameter.Type.Float:
						{
							EditorGUILayout.PropertyField(typeProperty);

							EditorGUILayout.PropertyField(condisionProperty.FindPropertyRelative("_FloatValue"), new GUIContent("Float Value"));
						}
						break;
					case Parameter.Type.Bool:
						{
							EditorGUILayout.PropertyField(condisionProperty.FindPropertyRelative("_BoolValue"), new GUIContent("Bool Value"));
						}
						break;
					case Parameter.Type.GameObject:
						{
							EditorGUILayout.PropertyField(condisionProperty.FindPropertyRelative("_GameObjectValue"), new GUIContent("GameObject Value"));
						}
						break;
				}
			}
		}

		void OnDrawChild(SerializedProperty property)
		{
			SerializedProperty referenceProperty = property.FindPropertyRelative("_Reference");

			EditorGUILayout.PropertyField(referenceProperty);

			SerializedProperty containerProperty = referenceProperty.FindPropertyRelative("container");

			ParameterContainerBase containerBase = containerProperty.objectReferenceValue as ParameterContainerBase;
			ParameterContainer container = null;
			if (containerBase != null)
			{
				container = containerBase.defaultContainer as ParameterContainer;
			}

			if (container != null)
			{
				CondisionGUI(container, referenceProperty, property);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ListGUI listGUI = new ListGUI(serializedObject.FindProperty("_Condisions"));

			listGUI.drawChild = OnDrawChild;

			listGUI.OnGUI();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
