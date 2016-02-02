using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
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