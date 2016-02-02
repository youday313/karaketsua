using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbody2DRotation")]
	[BuiltInBehaviour]
	public class TweenRigidbody2DRotation : TweenBase
	{
		[SerializeField] private Rigidbody2D _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private float _From;
		[SerializeField] private float _To;

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
				_Target = GetComponent<Rigidbody2D>();
			}
		}

		float _StartRotation;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target != null)
			{
				_StartRotation = _Target.rotation;
			}
			else
			{
				_StartRotation = 0.0f;
            }
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.MoveRotation( _StartRotation + Mathf.Lerp(_From, _To, factor) );
			}
		}
	}
}