using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnTriggerEnterStore")]
	[BuiltInBehaviour]
	public class OnTriggerEnterStore : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";
		[SerializeField]
		private GameObjectParameterReference _Parameter;

		void OnTriggerEnter(Collider collider)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || collider.tag == _Tag)
			{
				if (_Parameter.parameter != null)
				{
					_Parameter.parameter.gameObjectValue = collider.gameObject;
                }
			}
		}
	}
}
