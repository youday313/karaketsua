using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/ButtonTransition")]
	[BuiltInBehaviour]
	public class ButtonTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private string _ButtonName = "Fire1";

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetButton( _ButtonName ) )
			{
				Transition( _NextState );
			}	
		}
	}
}