using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("ButtonUpTransition")]
	[AddBehaviourMenu("Transition/Input/ButtonUpTransition")]
	[BuiltInBehaviour]
	public class ButtonUpTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private string _ButtonName = "Fire1";

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetButtonUp( _ButtonName ) )
			{
				Transition( _NextState );
			}	
		}
	}
}