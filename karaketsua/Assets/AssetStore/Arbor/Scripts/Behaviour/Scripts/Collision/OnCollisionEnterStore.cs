using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision/OnCollisionEnterStore")]
	[BuiltInBehaviour]
	public class OnCollisionEnterStore : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";
		[SerializeField]
		private GameObjectParameterReference _Parameter;

		void OnCollisionEnter(Collision collision)
		{
			if (!enabled)
			{
				return;
			}

			if (!_IsCheckTag || collision.gameObject.tag == _Tag)
			{
				if (_Parameter.parameter != null)
				{
					_Parameter.parameter.gameObjectValue = collision.gameObject;
                }
			}
		}
	}
}
