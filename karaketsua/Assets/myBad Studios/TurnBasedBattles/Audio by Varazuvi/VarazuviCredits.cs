using UnityEngine;
using System.Collections;

/// <summary>
/// Audio is provided by www.varazuvi.com
/// </summary>
public class VarazuviCredits : MonoBehaviour {


	string credits;

	public string 
		track = "Battle of the strings", 
		date = "2014", 
		artist = "The Artist";

	public Rect
		pos;

	public GUIStyle
		style;

	// Use this for initialization
	void Start () {
		credits = string.Format("Audio provided by Varazuvi.com:\n{0} Copyright © {1} {2}", track, date, artist);
	}
	
	// Update is called once per frame
	void OnGUI () {
		MBS.GUIX.FixScreenSize();
		GUI.Label(pos, credits, style);
	}
}
