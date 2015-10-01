using UnityEngine;
using System.Collections;

public enum SFXGSpawnLocation { Origin, Destination }

[System.Serializable]
public class SFXGEffectAndLocation
{
	public Transform	
		effect;

	public SFXGSpawnLocation	
		spawn_location;
}

public class SFXGDemoScript : MonoBehaviour {

	public SFXGEffectAndLocation[]	
		effects;


	public Transform
		origin, destination;

	public GUIStyle		
		button_style;

	public float
		timer_delay = 7f;

	float enabled_from;

	Vector2 scrollpos = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 5f, Screen.width * 0.4f, Screen.height * 0.53f));
		scrollpos = GUILayout.BeginScrollView(scrollpos);
		if ( Time.time < enabled_from )
		{
			GUILayout.Label("Buttons ready in " + (int)(enabled_from - Time.time + 1));
			GUI.enabled = false;
		} else
		{
			GUILayout.Label("Select an effect");
		}

		foreach(SFXGEffectAndLocation t in effects)
		{
			if (GUILayout.Button(t.effect.transform.name.Replace("Prefab","")))
			{
				enabled_from = Time.time + timer_delay;
				Transform e = (Transform)Instantiate (t.effect);
				if (t.spawn_location == SFXGSpawnLocation.Destination)
					e.position = destination.position;
				else
					e.position = origin.position;
				Destroy(e.gameObject, timer_delay);
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
