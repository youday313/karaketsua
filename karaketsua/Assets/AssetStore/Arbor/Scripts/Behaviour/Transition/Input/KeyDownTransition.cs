using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/KeyDownTransition")]
	[BuiltInBehaviour]
	public class KeyDownTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private KeyCode _KeyCode;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetKeyDown( _KeyCode ) )
			{
				Transition( _NextState );
			}
		}
	}
}