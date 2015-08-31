using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("TimeTransition")]
	[AddBehaviourMenu("Transition/TimeTransition")]
	[BuiltInBehaviour]
	public class TimeTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] public float _Seconds;

		private float _BeginTime;

		public override void OnStateBegin()
		{
			_BeginTime = Time.time;
		}

		void Update()
		{
			if( Time.time - _BeginTime >= _Seconds )
			{
				Transition( _NextState );
			}
		}
	}
}