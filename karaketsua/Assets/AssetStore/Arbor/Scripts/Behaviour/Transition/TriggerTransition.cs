using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("TriggerTransition")]
	[AddBehaviourMenu("Transition/TriggerTransition")]
	[BuiltInBehaviour]
	public class TriggerTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private string _Message;

		void OnStateTrigger( string message )
		{
			if( !enabled )
			{
				return;
			}

			if( _Message == message )
			{
				Transition( _NextState );
			}
		}
	}
}