using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
static public class tbbLayerManager {

	static tbbLayerManager()
	{
		EditorApplication.update += RunOnce;
	}
		
	private static void RunOnce()
	{
		EditorApplication.update -= RunOnce;
		CheckMyLayers();
	}

	private static void CheckMyLayers()
	{
		bool tags_modified = false;
		string[] RequiredLayers = new string[]{"TextOverlays"};
		string[] RequiredTags = new string[]{};

		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty tags = tagManager.FindProperty("tags");

		foreach (string s in RequiredTags)
		{
			// check if the tag is defined
			bool found = false;
			for (int i = 0; i < tags.arraySize; i++)
			{
				SerializedProperty t = tags.GetArrayElementAtIndex(i);
				if (t.stringValue.Equals(s)) { found = true; break; }
			}
			
			// if not found, add it
			if (!found)
			{
				tags.InsertArrayElementAtIndex(0);
				SerializedProperty n = tags.GetArrayElementAtIndex(0);
				n.stringValue = s;
				tags_modified = true;
			}
		}
		

		foreach( string s in RequiredLayers)
		{
			bool defined = false;
			for (int i = 8; i <= 31; i++)
			{
				string nm = "User Layer " + i;
				SerializedProperty sp = tagManager.FindProperty(nm);
				if (sp != null)
				{
					if (s == sp.stringValue)
					{
						defined = true;
						break;
					}
				}
			}
		
			if (!defined)
			{
				// not defined, find a spot for it
				for (int i = 8; i <= 31; i++)
				{
					string nm = "User Layer " + i;
					SerializedProperty sp = tagManager.FindProperty(nm);
					if (sp != null)
					{
						if (string.IsNullOrEmpty(sp.stringValue))
						{
							sp.stringValue = s;
							tags_modified = true;
							break;
						}
					}
				}
			}
		}

		// *** save changes
		if (tags_modified)
			tagManager.ApplyModifiedProperties();
	}
	
}