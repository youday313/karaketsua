using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("GoToTransition")]
	[AddBehaviourMenu("Transition/GoToTransition")]
	[BuiltInBehaviour]
	public class GoToTransition : StateBehaviour 
	{
		[SerializeField] private StateLink _NextState;

		// Use this for enter state
		public override void OnStateBegin()
		{
			Transition( _NextState );
		}
	}
}