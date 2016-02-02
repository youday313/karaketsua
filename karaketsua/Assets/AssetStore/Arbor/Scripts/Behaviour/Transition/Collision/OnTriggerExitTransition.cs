using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision/OnTriggerExitTransition")]
	[BuiltInBehaviour]
	public class OnTriggerExitTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";
		
		void OnTriggerExit( Collider collider )
		{
			if( !enabled )
			{
				return;
			}

			if( !_IsCheckTag || collider.tag == _Tag )
			{
				Transition( _NextState );
			}
		}
	}
}