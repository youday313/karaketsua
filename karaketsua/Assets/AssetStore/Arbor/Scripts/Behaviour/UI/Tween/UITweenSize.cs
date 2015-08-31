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
				Vector2 size = Vector2.Lerp(_From, _To, factor);

				_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
				_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
		}
	}
}