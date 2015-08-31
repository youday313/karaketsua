using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("DestroyGameObject")]
	[AddBehaviourMenu("GameObject/DestroyGameObject")]
	[BuiltInBehaviour]
    public class DestroyGameObject : StateBehaviour
	{
		[SerializeField] private GameObject _Target;

		public override void OnStateBegin()
		{
			if( _Target != null )
			{
				GameObject.Destroy ( _Target );
			}
		}
	}
}