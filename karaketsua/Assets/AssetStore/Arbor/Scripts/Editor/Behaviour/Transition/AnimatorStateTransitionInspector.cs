using UnityEngine;
using UnityEditor;
#if UNITY_5
using UnityEditor.Animations;
#else
using UnityEditorInternal;
#endif
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.AnimatorStateTransition))]
	public class AnimatorStateTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty animatorProperty = serializedObject.FindProperty("animator");
			SerializedProperty layerNameProperty = serializedObject.FindProperty("layerName");
			SerializedProperty stateNameProperty = serializedObject.FindProperty("stateName");

			EditorGUILayout.PropertyField(animatorProperty);

			Animator animator = animatorProperty.objectReferenceValue as Animator;

			if (animator != null && animator.runtimeAnimatorController != null )
			{
				AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

				int selected = -1;
				List<string> layerNames = new List<string>();

#if UNITY_5
				for (int i = 0; i < animatorController.layers.Length; i++)
				{
					AnimatorControllerLayer layer = animatorController.layers[i];

					layerNames.Add(layer.name);

					if (layer.name == layerNameProperty.stringValue)
					{
						selected = i;
					}
				}
#else
				for (int i = 0; i < animatorController.layerCount; i++)
				{
					AnimatorControllerLayer layer = animatorController.GetLayer(i);

					layerNames.Add(layer.name);

					if (layer.name == layerNameProperty.stringValue)
					{
						selected = i;
					}
				}
#endif

				selected = EditorGUILayout.Popup(ObjectNames.NicifyVariableName(layerNameProperty.name), selected, layerNames.ToArray());

				if (selected >= 0)
				{
					layerNameProperty.stringValue = layerNames[selected];

					List<string> stateNames = new List<string>();
#if UNITY_5
					AnimatorControllerLayer layer = animatorController.layers[selected];

					selected = -1;
					for (int i=0;i<layer.stateMachine.states.Length;i++)
					{
						AnimatorState state = layer.stateMachine.states[i].state;

						stateNames.Add(state.name);

						if (state.name == stateNameProperty.stringValue)
						{
							selected = i;
						}
					}
#else
					AnimatorControllerLayer layer = animatorController.GetLayer(selected);

					selected = -1;
					for (int i = 0; i < layer.stateMachine.stateCount; i++)
					{
						UnityEditorInternal.State state = layer.stateMachine.GetState(i);

						stateNames.Add(state.name);

						if (state.name == stateNameProperty.stringValue)
						{
							selected = i;
						}
					}
#endif

					selected = EditorGUILayout.Popup(ObjectNames.NicifyVariableName(stateNameProperty.name), selected, stateNames.ToArray());

					if (selected >= 0)
					{
						stateNameProperty.stringValue = stateNames[selected];
					}
				}
			}
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
