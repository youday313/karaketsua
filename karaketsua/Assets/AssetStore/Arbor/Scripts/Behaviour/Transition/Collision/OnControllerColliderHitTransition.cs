using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision/OnControllerColliderHitTransition")]
	[BuiltInBehaviour]
	public class OnControllerColliderHitTransition : StateBehaviour
	{
		[SerializeField]
		private StateLink _NextState;
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || hit.gameObject.tag == _Tag)
			{
				Transition(_NextState);
			}
		}
	}
}