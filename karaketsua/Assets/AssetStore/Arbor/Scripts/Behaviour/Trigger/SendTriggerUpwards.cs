using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/SendTriggerUpwards")]
	[BuiltInBehaviour]
	public class SendTriggerUpwards : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("_Target")]
		[SerializeField]
		private GameObject _OldTarget;

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

		public override void OnStateBegin()
		{
			GameObject current = target;
			while (current != null && current.activeInHierarchy)
			{
				foreach (ArborFSM fsm in current.GetComponents<ArborFSM>())
				{
					if (fsm.enabled)
					{
						fsm.SendTrigger(_Message);
					}
				}

				if (current.transform.parent != null)
				{
					current = current.transform.parent.gameObject;
				}
				else
				{
					current = null;
				}
			}
		}
	}
}
