using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/CalcParameter")]
	[BuiltInBehaviour]
	public class CalcParameter : StateBehaviour, ISerializationCallbackReceiver
	{
		public ParameterReference reference;

		public enum Function
		{
			Assign,
			Add,
		}
		public Function function;

		[FormerlySerializedAs("intValue")]
		[SerializeField] private int _OldIntValue;
		[FormerlySerializedAs("floatValue")]
		[SerializeField] private float _OldFloatValue;
		[FormerlySerializedAs("boolValue")]
		[SerializeField] private bool _OldBoolValue;

		[SerializeField] private int _SerializeVersion;
		[SerializeField] private FlexibleInt _IntValue;
		[SerializeField] private FlexibleFloat _FloatValue;
		[SerializeField] private FlexibleBool _BoolValue;
		[SerializeField] private FlexibleGameObject _GameObjectValue;

		public int intValue
		{
			get
			{
				return _IntValue.value;
			}
		}

		public float floatValue
		{
			get
			{
				return _FloatValue.value;
			}
		}

		public bool boolValue
		{
			get
			{
				return _BoolValue.value;
			}
		}

		public GameObject gameObjectValue
		{
			get
			{
				return _GameObjectValue.value;
			}
		}

		void SerializeVer1()
		{
			_IntValue = (FlexibleInt)_OldIntValue;
			_FloatValue = (FlexibleFloat)_OldFloatValue;
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

		// Use this for enter state
		public override void OnStateBegin()
		{
			Parameter parameter = reference.parameter;

			if (parameter == null)
			{
				return;
			}

			switch (parameter.type)
			{
				case Parameter.Type.Int:
					{
						int value = parameter.intValue;
                        switch (function)
						{
							case Function.Assign:
								value = intValue;
								break;
							case Function.Add:
								value += intValue;
								break;
						}
						if (parameter.intValue != value)
						{
							parameter.intValue = value;
							parameter.OnChanged();
                        }
					}
					break;
				case Parameter.Type.Float:
					{
						float value = parameter.floatValue;
						switch (function)
						{
							case Function.Assign:
								value = floatValue;
								break;
							case Function.Add:
								value += floatValue;
								break;
						}
						if (parameter.floatValue != value)
						{
							parameter.floatValue = value;
							parameter.OnChanged();
						}
					}
					break;
				case Parameter.Type.Bool:
					{
						bool value = boolValue;

						if (parameter.boolValue != value)
						{
							parameter.boolValue = value;
							parameter.OnChanged();
						}
					}
					break;
				case Parameter.Type.GameObject:
					{
						GameObject value = gameObjectValue;

						if (parameter.gameObjectValue != value)
						{
							parameter.gameObjectValue = value;
							parameter.OnChanged();
						}
					}
					break;
			}
		}
	}
}
