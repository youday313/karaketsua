using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision2D/OnTriggerExit2DTransition")]
	[BuiltInBehaviour]
	public class OnTriggerExit2DTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";
		
		void OnTriggerExit2D( Collider2D collider )
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