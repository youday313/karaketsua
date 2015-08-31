using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("OnTriggerEnterTransition")]
	[AddBehaviourMenu("Transition/Collision/OnTriggerEnterTransition")]
	[BuiltInBehaviour]
	public class OnTriggerEnterTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";

		void OnTriggerEnter( Collider collider )
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