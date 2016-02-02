using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/LookAtGameObject")]
	[BuiltInBehaviour]
	public class LookAtGameObject : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Transform")]
		[SerializeField] private Transform _OldTransform;

		[FormerlySerializedAs("_Target")]
		[SerializeField] private Transform _OldTarget;
		[SerializeField] private bool _ApplyLateUpdate;

		[SerializeField] private int _SerializeVersion;
		[SerializeField] private FlexibleGameObject _Transform;
		[SerializeField] private FlexibleGameObject _Target;

		public Transform target
		{
			get
			{
				if (_Target.value != null)
				{
					return _Target.value.transform;
				}
				return null;
			}
		}

		private Transform _CachedTransform;
		public Transform cachedTransform
		{
			get
			{
				if (_CachedTransform == null)
				{
					if (_Transform.value != null)
					{
						_CachedTransform = _Transform.value.transform;
					}
					else
					{
						_CachedTransform = transform;
                    }
				}
				return _CachedTransform;
			}
		}

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

		void LookAt()
		{
			if (cachedTransform != null && target != null)
			{
				cachedTransform.LookAt(target);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			LookAt();
        }

		void LateUpdate()
		{
			if (_ApplyLateUpdate)
			{
				LookAt();
			}
		}
	}
}
