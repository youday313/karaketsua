using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenColor")]
	[BuiltInBehaviour]
	public class TweenColor : TweenBase
	{
		[SerializeField] private Renderer _Target;
		[SerializeField] private Gradient _Gradient = new Gradient();

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Renderer>();
			}
		}

		protected override void OnTweenUpdate (float factor)
		{
			if( _Target != null && _Gradient != null )
			{
				_Target.material.color = _Gradient.Evaluate( factor );
			}
		}
	}
}