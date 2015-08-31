using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentEscape")]
	[BuiltInBehaviour]
	public class AgentEscape : StateBehaviour
	{
		[SerializeField] private AgentController _AgentController;
		[SerializeField] private float _Speed;
		[SerializeField] private float _Distance;
		[SerializeField] private Transform _Target;
		[SerializeField] private float _MinInterval;
		[SerializeField] private float _MaxInterval;

		private float _Timer;

		void Start()
		{
			if( _AgentController == null )
			{
				_AgentController = GetComponent<AgentController>();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			_Timer = 0.0f;
		}

		// Update is called once per frame
		void Update()
		{
			_Timer -= Time.deltaTime;

			if (_Timer <= 0.0f)
			{
				if (_AgentController != null)
				{
					_AgentController.Escape(_Speed, _Distance, _Target);
				}
				_Timer = Random.Range(_MinInterval, _MaxInterval);
			}
		}
	}
}