using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbodyPosition")]
	[BuiltInBehaviour]
	public class TweenRigidbodyPosition : TweenBase
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

		Vector3 _StartPosition;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target !=null)
			{
				_StartPosition = _Target.position;
			}
			else
			{
				_StartPosition = Vector3.zero;
            }
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.MovePosition(_StartPosition + Vector3.Lerp(_From, _To, factor) );
			}
		}
	}
}