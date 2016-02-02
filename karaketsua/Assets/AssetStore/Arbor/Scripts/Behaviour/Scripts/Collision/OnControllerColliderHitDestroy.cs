using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnControllerColliderHitDestroy")]
	[BuiltInBehaviour]
	public class OnControllerColliderHitDestroy : StateBehaviour
	{
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
				Destroy(hit.gameObject);
			}
		}
	}
}