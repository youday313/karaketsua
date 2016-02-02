using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SendMessageGameObject")]
	[BuiltInBehaviour]
	public class SendMessageGameObject : StateBehaviour, ISerializationCallbackReceiver
	{
		public enum Type
		{
			None,
			Int,
			Float,
			Bool,
			String,
		}
		[FormerlySerializedAs("_Target")]
		[SerializeField] private GameObject _OldTarget;

		[SerializeField] private string _MethodName;
		[SerializeField] private Type _Type;

		[FormerlySerializedAs("_IntValue")]
		[SerializeField] private int _OldIntValue;

		[FormerlySerializedAs("_FloatValue")]
		[SerializeField] private float _OldFloatValue;

		[FormerlySerializedAs("_BoolValue")]
		[SerializeField] private bool _OldBoolValue;

		[SerializeField] private string _StringValue;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleGameObject _Target;
		[SerializeField]
		private FlexibleFloat _FloatValue;
		[SerializeField]
		private FlexibleInt _IntValue;
		[SerializeField]
		private FlexibleBool _BoolValue;

		public GameObject target
		{
			get
			{
				return _Target.value;
			}
		}

		public float floatValue
		{
			get
			{
				return _FloatValue.value;
			}
		}

		public int intValue
		{
			get
			{
				return _IntValue.value;
			}
		}

		public bool boolValue
		{
			get
			{
				return _BoolValue.value;
			}
		}

		void SerializeVer1()
		{
			_Target = (FlexibleGameObject)_OldTarget;
			_FloatValue = (FlexibleFloat)_OldFloatValue;
			_IntValue = (FlexibleInt)_OldIntValue;
			_BoolValue = (FlexibleBool)_OldBoolValue;
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
			if( target != null )
			{
				switch (_Type)
				{
					case Type.None:
						target.SendMessage(_MethodName, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Int:
						target.SendMessage(_MethodName, intValue,SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Float:
						target.SendMessage(_MethodName, floatValue, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.Bool:
						target.SendMessage(_MethodName, boolValue, SendMessageOptions.DontRequireReceiver);
						break;
					case Type.String:
						target.SendMessage(_MethodName, _StringValue, SendMessageOptions.DontRequireReceiver);
						break;
				}
			}
		}
	}
}
