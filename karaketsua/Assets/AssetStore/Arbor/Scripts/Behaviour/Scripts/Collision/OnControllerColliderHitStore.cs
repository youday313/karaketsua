using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnControllerColliderHitStore")]
	[BuiltInBehaviour]
	public class OnControllerColliderHitStore : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";
		[SerializeField]
		private GameObjectParameterReference _Parameter;

		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || hit.gameObject.tag == _Tag)
			{
				if (_Parameter.parameter != null)
				{
					_Parameter.parameter.gameObjectValue = hit.gameObject;
                }
			}
		}
	}
}
