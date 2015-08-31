using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/AnimatorStateTransition")]
	[BuiltInBehaviour]
	public class AnimatorStateTransition : StateBehaviour
	{
		public Animator animator;
		public string layerName;
		public string stateName;
		public StateLink nextState;

		private int _LayerIndex = -1;

		void Awake()
		{
			if (animator == null)
			{
				return;
			}

			_LayerIndex = -1;

			for (int i = 0; i < animator.layerCount; i++)
			{
				if (animator.GetLayerName(i) == layerName)
				{
					_LayerIndex = i;
					break;
				}
			}
		}

		void CheckTransition()
		{
			if (animator != null && _LayerIndex >= 0 )
			{
				AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(_LayerIndex);
				if (stateInfo.IsName(layerName + "." + stateName))
				{
					Transition(nextState);
				}
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CheckTransition();
        }

		// Update is called once per frame
		void Update()
		{
			CheckTransition();
        }
	}
}