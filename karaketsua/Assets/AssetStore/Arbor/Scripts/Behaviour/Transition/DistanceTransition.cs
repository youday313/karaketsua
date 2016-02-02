using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/DistanceTransition")]
	[BuiltInBehaviour]
	public class DistanceTransition : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Target")]
		[SerializeField] private Transform _OldTarget;
		[SerializeField] private float _Distance;
		[SerializeField] private StateLink _NearState;
		[SerializeField] private StateLink _FarState;

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

		Transform _Transform;

		void Start()
		{
			_Transform = transform;
		}

		void Update () 
		{
			if( _Target.value == null )
			{
				return;
			}

			float distance = (_Transform.position- _Target.value.transform.position).sqrMagnitude;

			if( distance <= _Distance * _Distance )
			{
				Transition( _NearState );
			}
			else
			{
				Transition( _FarState );
			}
		}
	}
}
