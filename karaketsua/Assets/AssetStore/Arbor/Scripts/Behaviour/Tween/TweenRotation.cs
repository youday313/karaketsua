using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRotation")]
	[BuiltInBehaviour]
	public class TweenRotation : TweenBase
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
				_Target.localRotation = Quaternion.Euler(Vector3.Lerp(_From, _To, factor));
			}
		}
	}
}