using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/AnyKeyTransition")]
	[BuiltInBehaviour]
	public class AnyKeyTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _CheckOn = true;

		// Update is called once per frame
		void Update () 
		{
			if( Input.anyKey == _CheckOn )
			{
				Transition ( _NextState );
			}	
		}
	}
}