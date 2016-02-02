using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/ExistsGameObjectTransition")]
	[BuiltInBehaviour]
	public class ExistsGameObjectTransition : StateBehaviour
	{
		[SerializeField] private GameObject[] _Targets;
		[SerializeField] private bool _CheckUpdate;
		[SerializeField] private StateLink _AllExistsState;
		[SerializeField] private StateLink _AllNothingState;

		void CheckTransition()
		{
			if (_Targets.Length > 0)
			{
				int existsCount = 0;
				int nothingCount = 0;

				foreach (GameObject target in _Targets)
				{
					if (target != null)
					{
						existsCount++;
					}
					else
					{
						nothingCount++;
					}
                }
				if (existsCount == _Targets.Length)
				{
					Transition(_AllExistsState);
				}
				else if (nothingCount == _Targets.Length)
				{
					Transition(_AllNothingState);
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
			if (_CheckUpdate)
			{
				CheckTransition();
            }
		}
	}
}