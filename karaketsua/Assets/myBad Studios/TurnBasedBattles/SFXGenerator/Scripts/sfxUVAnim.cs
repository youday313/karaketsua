using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// This will animate the UVs on an object while adhering to the play_mode settings
	/// </summary>
	public class sfxUVAnim : sfxBaseAction 
	{
		/// <summary>
		/// The renderer of the object to animate
		/// </summary>
		public Renderer
			this_renderer;

		override public void Setup()
		{
			if (null == this_renderer)
				this_renderer = GetComponentInChildren<Renderer>();
			if (null == this_renderer)
				enabled = false;
		}

		// Update is called once per frame
		override public void OnUpdate () 
		{
			this_renderer.material.mainTextureOffset = new Vector2(value_current.x, value_current.y);
		}
	}
}
