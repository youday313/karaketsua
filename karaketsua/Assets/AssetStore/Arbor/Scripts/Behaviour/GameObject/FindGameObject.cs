using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/FindGameObject")]
	[BuiltInBehaviour]
	public class FindGameObject : StateBehaviour
	{
		[SerializeField]
		private GameObjectParameterReference _Reference;
		[SerializeField]
		private string _Name;

		public override void OnStateBegin()
		{
			if (_Reference.parameter != null)
			{
				_Reference.parameter.gameObjectValue = GameObject.Find(_Name);
            }
		}
	}
}
