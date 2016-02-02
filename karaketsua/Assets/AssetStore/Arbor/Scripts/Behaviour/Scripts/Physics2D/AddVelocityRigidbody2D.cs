using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics2D/AddVelocityRigidbody2D")]
	[BuiltInBehaviour]
	public class AddVelocityRigidbody2D : StateBehaviour
	{
		[FormerlySerializedAs("_Rigidbody2D")]
		[SerializeField]
		private Rigidbody2D _OldRigidbody2D;
		[SerializeField] private float _Angle;
		[SerializeField] private float _Speed;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;

		private Rigidbody2D _CachedRigidbody2D;
		public Rigidbody2D cachedRigidbody2D
		{
			get
			{
				if (_CachedRigidbody2D == null)
				{
					if (_Target.value != null)
					{
						_CachedRigidbody2D = _Target.value.GetComponent<Rigidbody2D>();
					}
					else
					{
						_CachedRigidbody2D = GetComponent<Rigidbody2D>();
					}
				}
				return _CachedRigidbody2D;
			}
		}

		void SerializeVer1()
		{
			if (_OldRigidbody2D != null)
			{
				_Target = (FlexibleGameObject)_OldRigidbody2D.gameObject;
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
			if (cachedRigidbody2D != null)
			{
				Vector2 direction = Quaternion.Euler(0.0f, 0.0f, _Angle) * cachedRigidbody2D.transform.up;
				cachedRigidbody2D.velocity = cachedRigidbody2D.velocity + direction.normalized * _Speed;
            }
		}
	}
}
