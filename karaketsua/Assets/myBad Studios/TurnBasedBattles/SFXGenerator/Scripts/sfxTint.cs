using UnityEngine;
using System.Collections;

namespace MBS {
	/// <summary>
	/// This will tint an object while adhering to the play_mode settings
	/// </summary>
	public class sfxTint : MonoBehaviour {

		/// <summary>
		/// The colors this animation should cycle through
		/// </summary>
		public Color[]
			colors;

		public float 
			/// <summary>
			/// How long before this effect starts
			/// </summary>
			start_delay,
			/// <summary>
			/// How long does this effect have to cycle through all colors?
			/// </summary>
			available_time;

		/// <summary>
		/// The renderer of the object to tint
		/// </summary>
		public Renderer
			this_renderer;

		float
			lerp_speed = 0,
			lerp_pos = 0,
			start_time;

		int index = 1;

		void Start()
		{
			if (null == this_renderer)
				this_renderer = GetComponentInChildren<Renderer>();
			if (null == this_renderer || null == colors)
				enabled = false;

			start_time = Time.time + start_delay;

			if (colors.Length > 0)
				this_renderer.material.color = colors[0];

			lerp_speed = available_time / colors.Length;
		}

		// Update is called once per frame
		void Update () 
		{
			if (index >= colors.Length)
			{
				enabled = false;
				return;
			}

			if (Time.time < start_time)
				return;

			lerp_pos  += Time.deltaTime;
			if (lerp_pos >= lerp_speed)
				lerp_pos = lerp_speed;

			this_renderer.material.color = Color.Lerp(colors[index -1], colors[index], lerp_pos / lerp_speed);
			if (lerp_pos == lerp_speed)
			{
				index++;
				lerp_pos = 0f;
			}
		}
	}
}
