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
		[SerializeField] private Vector3 _From = Vector3.one;
		[SerializeField] private Vector3 _To = Vector3.one;

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
				_Target.localScale = Vector3.Lerp(_From, _To, factor);
			}
		}
	}
}