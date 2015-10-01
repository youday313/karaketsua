using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
static class VitaInputManager {
	#region Input Manager Setup
	
	static VitaInputManager()
	{
		EditorApplication.update += RunOnce;
	}
	
	static void RunOnce()
	{
		EditorApplication.update -= RunOnce;
		SetupInputManager();
	}

	enum AxisType
	{
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	};
	
	class InputAxis
	{
		public string name;
		public string descriptiveName = "";
		public string descriptiveNegativeName = "";
		public string negativeButton = "";
		public string positiveButton = "";
		public string altNegativeButton = "";
		public string altPositiveButton = "";
		
		public float gravity = 0;
		public float dead = 0;
		public float sensitivity = 0;
		
		public bool snap = false;
		public bool invert = false;
		
		public AxisType type;
		
		public int axis;
		public int joyNum;
	}
	
	static void SetupInputManager()
	{
		// Add the Vita specific axis definitions
		AddAxis(new InputAxis() { name = "VitaLeftHorizontal",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis =  1, joyNum = 1 });
		AddAxis(new InputAxis() { name = "VitaLeftVertical",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis =  2, joyNum = 1 });
		AddAxis(new InputAxis() { name = "VitaRightHorizontal",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis =  4, joyNum = 1 });
		AddAxis(new InputAxis() { name = "VitaRightVertical",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis =  5, joyNum = 1 });
		AddAxis(new InputAxis() { name = "VitaLeftShoulder",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis =  8, joyNum = 1 });
		AddAxis(new InputAxis() { name = "VitaRightShoulder",	dead = 0.2f,sensitivity = 1f, type = AxisType.JoystickAxis, axis = 10, joyNum = 1 });
	}
	
	static void AddAxis(InputAxis axis)
	{
		if (AxisDefined(axis.name)) return;
		
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();
		
		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
		
		GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
		
		serializedObject.ApplyModifiedProperties();
	}
	
	static bool AxisDefined(string axisName)
	{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
		axesProperty.Next(true);
		axesProperty.Next(true);
		while (axesProperty.Next(false))
		{
			SerializedProperty axis = axesProperty.Copy();
			axis.Next(true);
			if (axis.stringValue == axisName) return true;
		}
		return false;
	}
	
	static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
	{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do
		{
			if (child.name == name) return child;
		}
		while (child.Next(false));
		return null;
	}
	
	#endregion
}
