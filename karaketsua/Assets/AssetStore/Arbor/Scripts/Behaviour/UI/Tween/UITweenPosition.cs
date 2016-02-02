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
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector2 _From;
		[SerializeField] private Vector2 _To;

		Vector2 _StartPosition;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<RectTransform>();
			}
		}

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative)
			{
				_StartPosition = _Target.anchoredPosition;
            }
			else
			{
				_StartPosition = Vector2.zero;
            }
		}

		protected override void OnTweenUpdate (float factor)
		{
			if(_Target != null )
			{
				_Target.anchoredPosition = _StartPosition + Vector2.Lerp(_From, _To, factor);
            }
		}
	}
}
