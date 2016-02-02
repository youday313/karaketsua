using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/KeyTransition")]
	[BuiltInBehaviour]
	public class KeyTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private KeyCode _KeyCode;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetKey( _KeyCode ) )
			{
				Transition( _NextState );
			}
		}
	}
}