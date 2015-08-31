using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("OnTriggerEnter2DTransition")]
	[AddBehaviourMenu("Transition/Collision2D/OnTriggerEnter2DTransition")]
	[BuiltInBehaviour]
	public class OnTriggerEnter2DTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";

		void OnTriggerEnter2D( Collider2D collider )
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