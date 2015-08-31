using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("MouseButtonUpTransition")]
	[AddBehaviourMenu("Transition/Input/MouseButtonUpTransition")]
	[BuiltInBehaviour]
	public class MouseButtonUpTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private int _Button;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetMouseButtonUp( _Button ) )
			{
				Transition ( _NextState );
			}	
		}
	}
}