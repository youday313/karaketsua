using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnTriggerExitDestroy")]
	[BuiltInBehaviour]
	public class OnTriggerExitDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnTriggerExit(Collider collider)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || collider.tag == _Tag)
			{
				Destroy(collider.gameObject);
			}
		}
	}
}