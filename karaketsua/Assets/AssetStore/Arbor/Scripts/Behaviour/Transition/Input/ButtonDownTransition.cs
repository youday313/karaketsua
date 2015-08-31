using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("ButtonDownTransition")]
	[AddBehaviourMenu("Transition/Input/ButtonDownTransition")]
	[BuiltInBehaviour]
	public class ButtonDownTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private string _ButtonName = "Fire1";

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetButtonDown( _ButtonName ) )
			{
				Transition( _NextState );
			}	
		}
	}
}