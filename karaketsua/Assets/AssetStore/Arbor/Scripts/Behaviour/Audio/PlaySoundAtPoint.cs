using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("PlaySoundAtPoint")]
	[AddBehaviourMenu("Audio/PlaySoundAtPoint")]
	[BuiltInBehaviour]
	public class PlaySoundAtPoint : StateBehaviour
	{
		[SerializeField] private AudioClip _Clip;
		[SerializeField] private Transform _Target;
		[SerializeField] private float _Volume = 1.0f;

		// Use this for enter state
		public override void OnStateBegin() 
		{
			Transform target = _Target;
			if( target == null )
			{
				target = transform;
			}
			AudioSource.PlayClipAtPoint( _Clip,target.position,_Volume );
		}
	}
}