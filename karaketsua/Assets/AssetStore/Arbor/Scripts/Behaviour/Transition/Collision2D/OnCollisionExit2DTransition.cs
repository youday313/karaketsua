using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision2D/OnCollisionExit2DTransition")]
	[BuiltInBehaviour]
	public class OnCollisionExit2DTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;
		[SerializeField] private bool _IsCheckTag;
		[SerializeField] private string _Tag = "Untagged";

		void OnCollisionExit2D( Collision2D collision )
		{
			if( !enabled )
			{
				return;
			}

			if( !_IsCheckTag || _Tag == collision.gameObject.tag )
			{
				Transition ( _NextState );
			}
		}
	}
}