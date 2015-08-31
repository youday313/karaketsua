using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("MouseButtonDownTransition")]
	[AddBehaviourMenu("Transition/Input/MouseButtonDownTransition")]
	[BuiltInBehaviour]
	public class MouseButtonDownTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private int _Button = 0;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetMouseButtonDown( _Button ) )
			{
				Transition( _NextState );
			}
		}
	}
}