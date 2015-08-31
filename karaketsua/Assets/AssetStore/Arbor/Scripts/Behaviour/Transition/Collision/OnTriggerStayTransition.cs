using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("OnTriggerStayTransition")]
	[AddBehaviourMenu("Transition/Collision/OnTriggerStayTransition")]
	[BuiltInBehaviour]
	public class OnTriggerStayTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";

		void OnTriggerStay( Collider collider )
		{
			if( !enabled )
			{
				return;
			}

			if( !_IsCheckTag || _Tag == collider.tag )
			{
				Transition( _NextState );
			}
		}
	}
}