using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/StopSound")]
	[BuiltInBehaviour]
	public class StopSound : StateBehaviour
	{
		[SerializeField]
		private AudioSource _AudioSource;

		void Awake()
		{
			if (_AudioSource == null)
			{
				_AudioSource = GetComponent<AudioSource>();
			}
		}

		void Stop()
		{
			if (_AudioSource != null)
			{
				_AudioSource.Stop();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Stop();
		}
	}
}