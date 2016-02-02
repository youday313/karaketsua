using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision/OnCollisionEnterTransition")]
	[BuiltInBehaviour]
	public class OnCollisionEnterTransition : StateBehaviour
	{
		[SerializeField]
		private StateLink _NextState;
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
				Transition(_NextState);
			}
		}
	}
}