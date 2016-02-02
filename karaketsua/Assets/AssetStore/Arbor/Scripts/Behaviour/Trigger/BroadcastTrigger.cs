using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/BroadcastTrigger")]
	[BuiltInBehaviour]
	public class BroadcastTrigger : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Target")]
		[SerializeField] private GameObject _OldTarget;
		[SerializeField] private string _Message;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;

		public GameObject target
		{
			get
			{
				return _Target.value;
            }
		}

		void SerializeVer1()
		{
			_Target = (FlexibleGameObject)_OldTarget;
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

		void Broadcast(GameObject target)
		{
			if (target != null && target.activeInHierarchy)
			{
				foreach (ArborFSM fsm in target.GetComponents<ArborFSM>())
				{
					if (fsm.enabled)
					{
						fsm.SendTrigger(_Message);
					}
				}

				foreach (Transform child in target.transform)
				{
					Broadcast(child.gameObject);
                }
			}
		}

		public override void OnStateBegin()
		{
			Broadcast(target);
		}
	}
}
