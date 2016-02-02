using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/OnCollisionExit2DDestroy")]
	[BuiltInBehaviour]
	public class OnCollisionExit2DDestroy : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";

		void OnCollisionExit2D(Collision2D collision)
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