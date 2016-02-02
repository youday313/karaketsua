using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collision2D/OnTriggerExit2DStore")]
	[BuiltInBehaviour]
	public class OnTriggerExit2DStore : StateBehaviour
	{
		[SerializeField]
		private bool _IsCheckTag;
		[SerializeField]
		private string _Tag = "Untagged";
		[SerializeField]
		private GameObjectParameterReference _Parameter;

		void OnTriggerExit2D(Collider2D collider)
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
