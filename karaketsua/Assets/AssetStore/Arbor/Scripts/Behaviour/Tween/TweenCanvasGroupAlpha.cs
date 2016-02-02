using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenCanvasGroupAlpha")]
	[BuiltInBehaviour]
	public class TweenCanvasGroupAlpha : TweenBase
	{
		[SerializeField] private CanvasGroup _Target;
		[SerializeField] private float _From;
		[SerializeField] private float _To;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<CanvasGroup>();
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_Target != null)
			{
				_Target.alpha = Mathf.Lerp(_From, _To, factor);
			}
		}
	}
}