using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SendEventGameObject")]
	[BuiltInBehaviour]
	public class SendEventGameObject : StateBehaviour
	{
		[System.Serializable]
		class IntEvent : UnityEvent<int>
		{
		}
		[System.Serializable]
		class FloatEvent : UnityEvent<float>
		{
		}
		[System.Serializable]
		class BoolEvent : UnityEvent<bool>
		{
		}
		[System.Serializable]
		class StringEvent : UnityEvent<string>
		{
		}

		public enum Type
		{
			None,
			Int,
			Float,
			Bool,
			String,
		}

		[SerializeField] private Type _Type;
		[SerializeField] private UnityEvent _Event;

		[SerializeField] private int _IntValue;
		[SerializeField] private IntEvent _IntEvent;

		[SerializeField] private float _FloatValue;
		[SerializeField] private FloatEvent _FloatEvent;

		[SerializeField] private bool _BoolValue;
		[SerializeField] private BoolEvent _BoolEvent;

		[SerializeField] private string _StringValue;
		[SerializeField] private StringEvent _StringEvent;

		// Use this for enter state
		public override void OnStateBegin()
		{
			switch (_Type)
			{
				case Type.None:
					if (_Event != null)
					{
						_Event.Invoke();
					}
					break;
				case Type.Int:
					if (_IntEvent != null)
					{
						_IntEvent.Invoke(_IntValue);
					}
					break;
				case Type.Float:
					if (_FloatEvent != null)
					{
						_FloatEvent.Invoke(_FloatValue);
					}
					break;
				case Type.Bool:
					if (_BoolEvent != null)
					{
						_BoolEvent.Invoke(_BoolValue);
					}
					break;
				case Type.String:
					if (_StringEvent != null)
					{
						_StringEvent.Invoke(_StringValue);
					}
					break;
			}
		}
	}
}