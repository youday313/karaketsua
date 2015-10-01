using UnityEngine;
using System.Collections;
using MBS;

public class DemoMenu : MonoBehaviour {

	public Texture
		logo,
		start_game_img,
		credits_img,
		credits_info_img,
		main_menu_img;

	public Rect
		logo_area,
		start_game_area,
		credits_area,
		credits_info_area;

	public GUISkin
		the_skin;

	bool credits = false;

	void Start()
	{
		GUIX.SetScreenSize(1024, 768);
	}

	void Update()
	{
		transform.Rotate (0, 5* Time.deltaTime, 0);
	}

	void OnGUI () {

		GUI.skin = the_skin;

		GUIX.FixScreenSize();
		if (credits)
		{
			if (credits_info_img)
				GUI.DrawTexture(credits_info_area, credits_info_img);

			if (GUI.Button(credits_area, main_menu_img))
				credits = !credits;
		} else
		{
			if (logo)
				GUI.DrawTexture(logo_area, logo);
		
			if (GUI.Button(start_game_area, start_game_img))
				Application.LoadLevel("TBBDemoScene");

			if (GUI.Button(credits_area, credits_img))
				credits = !credits;
		}

	}
}
