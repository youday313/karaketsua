using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// This will scale an object while adhering to the play_mode settings
	/// </summary>
	public class sfxScale : sfxBaseAction 
	{

		void Awake()
		{
			transform.localScale = value_start;
		}

		// Update is called once per frame
		override public void OnUpdate () 
		{
			transform.localScale = value_current;
		}
	}
}
