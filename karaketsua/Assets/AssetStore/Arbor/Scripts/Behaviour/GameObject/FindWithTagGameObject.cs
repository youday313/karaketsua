using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/FindWithTagGameObject")]
	[BuiltInBehaviour]
	public class FindWithTagGameObject : StateBehaviour
	{
		[SerializeField]
		private GameObjectParameterReference _Reference;
		[SerializeField]
		private string _Tag = "Untagged";

		public override void OnStateBegin()
		{
			if (_Reference.parameter != null)
			{
				_Reference.parameter.gameObjectValue = GameObject.FindWithTag(_Tag);
            }
		}
	}
}
