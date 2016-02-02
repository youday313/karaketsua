using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentEscape")]
	[BuiltInBehaviour]
	public class AgentEscape : StateBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private AgentController _AgentController;
		[SerializeField] private float _Speed;
		[SerializeField] private float _Distance;

		[FormerlySerializedAs("_Target")]
		[SerializeField] private Transform _OldTarget;

		[SerializeField] private float _MinInterval;
		[SerializeField] private float _MaxInterval;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;

		public Transform target
		{
			get
			{
				if (_Target.value != null)
				{
					return _Target.value.transform;
				}
				return null;
			}
		}

		void SerializeVer1()
		{
			if (_OldTarget != null)
			{
				_Target = (FlexibleGameObject)_OldTarget.gameObject;
			}
		}

		public void OnBeforeSerialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
				_SerializeVersion = 1;
			}
		}

		public void OnAfterDeserialize()
		{
			if (_SerializeVersion == 0)
			{
				SerializeVer1();
			}
		}

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
					_AgentController.Escape(_Speed, _Distance, target);
				}
				_Timer = Random.Range(_MinInterval, _MaxInterval);
			}
		}
	}
}
