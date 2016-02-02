using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnCollisionExitDestroy")]
	[BuiltInBehaviour]
	public class OnCollisionExitDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnCollisionExit(Collision collision)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || collision.gameObject.tag == _Tag)
			{
				Destroy(collision.gameObject);
			}
		}
	}
}