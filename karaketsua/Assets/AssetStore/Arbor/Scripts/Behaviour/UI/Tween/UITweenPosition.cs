using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/Tween/UITweenPosition")]
	[BuiltInBehaviour]
	public class UITweenPosition : TweenBase
	{
		[SerializeField] private RectTransform _Target;
		[SerializeField] private Vector2 _From;
		[SerializeField] private Vector2 _To;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<RectTransform>();
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if(_Target != null )
			{
				_Target.anchoredPosition = Vector2.Lerp(_From, _To, factor);
            }
		}
	}
}