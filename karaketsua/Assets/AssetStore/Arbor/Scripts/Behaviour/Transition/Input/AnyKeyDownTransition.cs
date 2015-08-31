using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("AnyKeyDownTransition")]
	[AddBehaviourMenu("Transition/Input/AnyKeyDownTransition")]
	[BuiltInBehaviour]
	public class AnyKeyDownTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _CheckDown = true;

		// Update is called once per frame
		void Update () 
		{
			if( Input.anyKeyDown == _CheckDown )
			{
				Transition ( _NextState );
			}	
		}
	}
}