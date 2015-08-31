using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("SendTrigger")]
	[AddBehaviourMenu("Trigger/SendTrigger")]
	[BuiltInBehaviour]
	public class SendTrigger : StateBehaviour
	{
		[SerializeField] private ArborFSM _Target;
		[SerializeField] private string _Message;

		public override void OnStateBegin()
		{
			if( _Target != null )
			{
				_Target.SendTrigger( _Message );
			}
		}
	}
}