using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// This will move an object while adhering to the play_mode settings
	/// </summary>
	public class sfxTransform : sfxBaseAction {

		/// <summary>
		/// Normally objects spawn at an object's origin which is normally at the feet. This will allow you to specify an arbitrary starting offset
		/// </summary>
		public Vector3
			offset_at_start = Vector3.zero;

		override public void Setup()
		{
			transform.Translate ( offset_at_start );
		}
		
		// Update is called once per frame
		override public void OnUpdate () 
		{
			if (play_mode == eSfxPlayMode.KeepGoing)
			{
				transform.Translate ( value_this_update );
			}
			else
			{
				transform.localPosition = value_current;
			}
		}
	}
}
