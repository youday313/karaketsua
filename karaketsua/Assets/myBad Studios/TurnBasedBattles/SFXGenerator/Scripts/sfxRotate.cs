using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// This will rotate an object while adhering to the play_mode settings
	/// </summary>
	public class sfxRotate : sfxBaseAction {

		// Update is called once per frame
		override public void OnUpdate () 
		{
			transform.localEulerAngles = value_current;
		}
	}
}
