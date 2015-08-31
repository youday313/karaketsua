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
		[SerializeField] private Vector3 _From;
		[SerializeField] private Vector3 _To;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Transform>();
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if (_Target != null)
			{
				_Target.localPosition = Vector3.Lerp(_From, _To, factor);
			}
		}
	}
}