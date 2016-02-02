using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/TimeTransition")]
	[BuiltInBehaviour]
	public class TimeTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] public float _Seconds;

		float _BeginTime = 0.0f;
		public float elapsedTime
		{
			get
			{
				return Time.time - _BeginTime;
			}
		}

		public override void OnStateBegin()
		{
			_BeginTime = Time.time;
			StartCoroutine(Wait());
		}

		void Update()
		{
			if (elapsedTime >= _Seconds)
			{
				Transition(_NextState);
			}
		}

		IEnumerator Wait()
		{
			yield return new WaitForSeconds(_Seconds);
			Transition(_NextState);
		}
	}
}