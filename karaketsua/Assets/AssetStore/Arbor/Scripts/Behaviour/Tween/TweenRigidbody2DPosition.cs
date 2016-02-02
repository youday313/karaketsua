using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbody2DPosition")]
	[BuiltInBehaviour]
	public class TweenRigidbody2DPosition : TweenBase
	{
		[SerializeField] private Rigidbody2D _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector2 _From;
		[SerializeField] private Vector2 _To;

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

		Vector2 _StartPosition;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target != null)
			{
				_StartPosition = _Target.position;
			}
			else
			{
				_StartPosition = Vector2.zero;
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.MovePosition(_StartPosition + Vector2.Lerp(_From, _To, factor) );
			}
		}
	}
}