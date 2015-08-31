using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("DistanceTransition")]
	[AddBehaviourMenu("Transition/DistanceTransition")]
	[BuiltInBehaviour]
	public class DistanceTransition : StateBehaviour
	{
		[SerializeField] private Transform _Target;
		[SerializeField] private float _Distance;
		[SerializeField] private StateLink _NearState;
		[SerializeField] private StateLink _FarState;

		Transform _Transform;

		void Start()
		{
			_Transform = transform;
		}

		void Update () 
		{
			if( _Target == null )
			{
				return;
			}

			float distance = (_Transform.position-_Target.position).sqrMagnitude;

			if( distance <= _Distance * _Distance )
			{
				Transition( _NearState );
			}
			else
			{
				Transition( _FarState );
			}
		}
	}
}