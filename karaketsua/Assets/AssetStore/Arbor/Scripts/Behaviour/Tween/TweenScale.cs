using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenScale")]
	[BuiltInBehaviour]
	public class TweenScale : TweenBase
	{
		[SerializeField] private Transform _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector3 _From = Vector3.one;
		[SerializeField] private Vector3 _To = Vector3.one;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Transform>();
			}
		}

		Vector3 _StartScale;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative && _Target != null)
			{
				_StartScale = _Target.localScale;
			}
			else
			{
				_StartScale = Vector3.zero;
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.localScale = _StartScale + Vector3.Lerp(_From, _To, factor);
			}
		}
	}
}