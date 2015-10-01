using UnityEngine;
using System.Collections;
using MBS;

public class DemoMainGame: MonoBehaviour {

	public Texture
		start_game_img;
	
	public Rect
		start_game_area;
	
	public GUISkin
		the_skin;
	
	void Start()
	{
		GUIX.SetScreenSize(1024, 768);
	}
	
	void Update()
	{
		transform.Rotate (0, 5* Time.deltaTime, 0);
	}
	
	void OnGUI () {

		GUI.Label(new Rect(15,15,500,50), "You are now in the main game... Click to go to the main menu");

		GUI.skin = the_skin;
		
		GUIX.FixScreenSize();
		if (GUI.Button(start_game_area, start_game_img))
			Application.LoadLevel("MainMenu");
	}
}
