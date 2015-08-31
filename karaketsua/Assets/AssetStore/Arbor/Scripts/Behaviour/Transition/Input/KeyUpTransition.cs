using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("KeyUpTransition")]
	[AddBehaviourMenu("Transition/Input/KeyUpTransition")]
	[BuiltInBehaviour]
	public class KeyUpTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private KeyCode _KeyCode;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetKeyUp( _KeyCode ) )
			{
				Transition( _NextState );
			}	
		}
	}
}