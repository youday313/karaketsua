using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(StateLink))]
	sealed class StateLinkPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position,SerializedProperty property,GUIContent label )
		{
		}
		
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return -2.0f;
		}
	}
}