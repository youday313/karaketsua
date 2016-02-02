using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbodyRotation")]
	[BuiltInBehaviour]
	public class TweenRigidbodyRotation : TweenBase
	{
		[SerializeField] private Rigidbody _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector3 _From;
		[SerializeField] private Vector3 _To;

		protected override bool fixedUpdate
		{
			get
			{
				return true;
			}
		}

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Rigidbody>();
			}
		}

		Quaternion _StartRotation;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target!=null)
			{
				_StartRotation = _Target.rotation;
			}
			else
			{
				_StartRotation = Quaternion.identity;
            }
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.MoveRotation(_StartRotation * Quaternion.Euler(Vector3.Lerp(_From, _To, factor)) );
			}
		}
	}
}