using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnCollisionEnterDestroy")]
	[BuiltInBehaviour]
	public class OnCollisionEnterDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnCollisionEnter(Collision collision)
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