using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRotation")]
	[BuiltInBehaviour]
	public class TweenRotation : TweenBase
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

		Quaternion _StartRotation;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target != null)
			{
				_StartRotation = _Target.localRotation;
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
				_Target.localRotation = _StartRotation * Quaternion.Euler(Vector3.Lerp(_From, _To, factor));
			}
		}
	}
}