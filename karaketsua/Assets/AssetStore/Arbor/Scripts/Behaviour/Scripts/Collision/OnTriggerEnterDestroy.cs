using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnTriggerEnterDestroy")]
	[BuiltInBehaviour]
	public class OnTriggerEnterDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnTriggerEnter(Collider collider)
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