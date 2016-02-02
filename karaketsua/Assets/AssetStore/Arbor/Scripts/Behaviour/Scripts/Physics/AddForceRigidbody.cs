using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics/AddForceRigidbody")]
	[BuiltInBehaviour]
	public class AddForceRigidbody : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Rigidbody")]
		[SerializeField] private Rigidbody _OldRigidbody;
		[SerializeField] private Vector3 _Angle;
		[SerializeField] private float _Power;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;

		private Rigidbody _CachedRigidbody;
		public Rigidbody cachedRigidbody
		{
			get
			{
				if (_CachedRigidbody == null)
				{
					if (_Target.value != null)
					{
						_CachedRigidbody = _Target.value.GetComponent<Rigidbody>();
					}
					else
					{
						_CachedRigidbody = GetComponent<Rigidbody>();
					}
				}
				return _CachedRigidbody;
			}
		}

		void SerializeVer1()
		{
			if (_OldRigidbody != null)
			{
				_Target = (FlexibleGameObject)_OldRigidbody.gameObject;
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
			if (cachedRigidbody != null)
			{
				Vector3 direction = Quaternion.Euler(_Angle) * cachedRigidbody.transform.forward;
				cachedRigidbody.AddForce( direction.normalized * _Power);
            }
		}
	}
}
