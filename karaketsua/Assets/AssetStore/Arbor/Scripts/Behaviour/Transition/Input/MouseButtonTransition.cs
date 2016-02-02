using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/MouseButtonTransition")]
	[BuiltInBehaviour]
	public class MouseButtonTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private int _Button = 0;

		// Update is called once per frame
		void Update () 
		{
			if( Input.GetMouseButton( _Button ) )
			{
				Transition( _NextState );
			}
		}
	}
}