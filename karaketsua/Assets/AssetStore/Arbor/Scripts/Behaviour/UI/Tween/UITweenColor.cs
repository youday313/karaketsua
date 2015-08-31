using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/Tween/UITweenColor")]
	[BuiltInBehaviour]
	public class UITweenColor : TweenBase
	{
		[SerializeField] private Graphic _Target;
		[SerializeField] private Gradient _Gradient = new Gradient();

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Graphic>();
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if(_Target != null && _Gradient != null )
			{
				_Target.color = _Gradient.Evaluate( factor );
			}
		}
	}
}