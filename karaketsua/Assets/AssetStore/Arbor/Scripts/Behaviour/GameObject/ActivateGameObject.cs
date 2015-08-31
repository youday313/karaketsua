using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("ActivateGameObject")]
	[AddBehaviourMenu("GameObject/ActivateGameObject")]
	[BuiltInBehaviour]
	public class ActivateGameObject : StateBehaviour
	{
		[SerializeField] private GameObject _Target;
		[SerializeField] private bool _BeginActive;
		[SerializeField] private bool _EndActive;

		public override void OnStateBegin()
		{
			if( _Target != null )
			{
				_Target.SetActive( _BeginActive );
			}
		}

		public override void OnStateEnd()
		{
			if( _Target != null )
			{
				_Target.SetActive( _EndActive );
			}
		}
	}
}