using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/PlaySoundAtPoint")]
	[BuiltInBehaviour]
	public class PlaySoundAtPoint : StateBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private AudioClip _Clip;

		[FormerlySerializedAs("_Target")]
		[SerializeField] private Transform _OldTarget;
		[SerializeField] private float _Volume = 1.0f;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;

		void SerializeVer1()
		{
			if (_OldTarget != null)
			{
				_Target = (FlexibleGameObject)_OldTarget.gameObject;
			}
		}

		public void OnBeforeSerialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
				_SerializeVersion = 1;
			}
		}

		public void OnAfterDeserialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
			}
		}

		// Use this for enter state
		public override void OnStateBegin() 
		{
			Transform target = null;
			if (_Target.value != null)
			{
				target = _Target.value.transform;
			}
			if( target == null )
			{
				target = transform;
			}
			AudioSource.PlayClipAtPoint( _Clip,target.position,_Volume );
		}
	}
}
