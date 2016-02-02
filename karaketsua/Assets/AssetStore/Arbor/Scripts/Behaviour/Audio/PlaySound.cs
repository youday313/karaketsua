using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/PlaySound")]
	[BuiltInBehaviour]
	public class PlaySound : StateBehaviour
	{
		[SerializeField]
		private AudioSource _AudioSource;
		[SerializeField]
		private bool _IsSetClip;
		[SerializeField]
		private AudioClip _Clip;

		void Awake()
		{
			if (_AudioSource == null)
			{
				_AudioSource = GetComponent<AudioSource>();
			}
		}

		void Play()
		{
			if (_AudioSource != null)
			{
				if (_IsSetClip)
				{
					_AudioSource.clip = _Clip;
				}
				_AudioSource.Play();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Play();
		}
	}
}