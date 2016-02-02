using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SendEventGameObject")]
	[BuiltInBehaviour]
	public class SendEventGameObject : StateBehaviour
	{
		[SerializeField] private UnityEvent _Event;

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Event != null)
			{
				_Event.Invoke();
			}
		}
	}
}