using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenPosition")]
	[BuiltInBehaviour]
	public class TweenPosition : TweenBase
	{
		[SerializeField] private Transform _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector3 _From;
		[SerializeField] private Vector3 _To;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Transform>();
			}
		}

		Vector3 _StartPosition;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target != null)
			{
				_StartPosition = _Target.localPosition;
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
				_Target.localPosition = _StartPosition + Vector3.Lerp(_From, _To, factor);
			}
		}
	}
}