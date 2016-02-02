using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/OnTriggerEnter2DDestroy")]
	[BuiltInBehaviour]
	public class OnTriggerEnter2DDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnTriggerEnter2D(Collider2D collider)
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