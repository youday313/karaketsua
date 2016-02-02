using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/Tween/UITweenSize")]
	[BuiltInBehaviour]
	public class UITweenSize : TweenBase
	{
		[SerializeField] private RectTransform _Target;
		[SerializeField] private bool _Relative;
		[SerializeField] private Vector2 _From;
		[SerializeField] private Vector2 _To;

		private Vector2 _StartSize;

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
				_StartSize = new Vector2(_Target.rect.width, _Target.rect.height);
			}
			else
			{
				_StartSize = Vector2.zero;
            }
		}

		protected override void OnTweenUpdate (float factor)
		{
			if(_Target != null )
			{
				Vector2 size = _StartSize + Vector2.Lerp(_From, _To, factor);

				_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
				_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
		}
	}
}
