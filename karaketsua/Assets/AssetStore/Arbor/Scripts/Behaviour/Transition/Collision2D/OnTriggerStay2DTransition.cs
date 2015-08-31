using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("OnTriggerStay2DTransition")]
	[AddBehaviourMenu("Transition/Collision2D/OnTriggerStay2DTransition")]
	[BuiltInBehaviour]
	public class OnTriggerStay2DTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";
		
		void OnTriggerStay2D( Collider2D collider )
		{
			if( !enabled )
			{
				return;
			}
			
			if( !_IsCheckTag || _Tag == collider.tag )
			{
				Transition ( _NextState );
			}
		}
	}
}