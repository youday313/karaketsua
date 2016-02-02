using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/ParameterTransition")]
	[BuiltInBehaviour]
	public class ParameterTransition : StateBehaviour, ISerializationCallbackReceiver
	{
		[System.Serializable]
		public class Condision
		{
			public enum Type
			{
				Greater,
				Less,
				Equals,
				NotEquals,
			}

			[FormerlySerializedAs("reference")]
			[SerializeField]
			private ParameterReference _Reference;

			public ParameterReference reference
			{
				get
				{
					return _Reference;
				}
			}

			[FormerlySerializedAs("type")]
			[SerializeField]
			private Type _Type;

			public Type type
			{
				get
				{
					return _Type;
				}
			}
			

			[FormerlySerializedAs("intValue")]
			[SerializeField]
			private int _OldIntValue;

			[FormerlySerializedAs("floatValue")]
			[SerializeField]
			private float _OldFloatValue;

			[FormerlySerializedAs("boolValue")]
			[SerializeField]
			private bool _OldBoolValue;
			
			[SerializeField] private FlexibleInt _IntValue;
			[SerializeField] private FlexibleFloat _FloatValue;
			[SerializeField] private FlexibleBool _BoolValue;
			[SerializeField] private FlexibleGameObject _GameObjectValue;

			public int intValue
			{
				get
				{
					return (int)_IntValue;
				}
			}

			public float floatValue
			{
				get
				{
					return (float)_FloatValue;
				}
			}

			public bool boolValue
			{
				get
				{
					return (bool)_BoolValue;
				}
			}

			public GameObject gameObjectValue
			{
				get
				{
					return (GameObject)_GameObjectValue;
				}
			}

			public void SerializeVer1()
			{
				_IntValue = (FlexibleInt)_OldIntValue;
				_FloatValue = (FlexibleFloat)_OldFloatValue;
				_BoolValue = (FlexibleBool)_OldBoolValue;
			}
		}

		[SerializeField] private StateLink _NextState;
		[SerializeField] private List<Condision> _Condisions = new List<Condision>();

		[SerializeField] private int _SerializeVersion;

		void SerializeVer1()
		{
			foreach (Condision condision in _Condisions)
			{
				condision.SerializeVer1();
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

		// Use this for enter state
		public override void OnStateBegin()
		{
			foreach (Condision condision in _Condisions)
			{
				Parameter parameter = condision.reference.parameter;
				if (parameter != null)
				{
					parameter.onChanged += OnChangedParam;
				}
            }

			if (CheckCondision())
			{
				Transition(_NextState);
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			foreach (Condision condision in _Condisions)
			{
				Parameter parameter = condision.reference.parameter;
				if (parameter != null)
				{
					parameter.onChanged -= OnChangedParam;
				}
			}
		}

		void OnChangedParam(Parameter parameter)
		{
			if ( CheckCondision() )
			{
				Transition(_NextState);
			}
		}

		bool CheckCondision()
		{
			int count = 0;
			int result = 0;

			foreach (Condision condision in _Condisions)
			{
				Parameter parameter = condision.reference.parameter;
				if (parameter == null)
				{
					continue;
				}

				count++;

				switch (parameter.type)
				{
					case Parameter.Type.Int:
						{
							switch (condision.type)
							{
								case Condision.Type.Greater:
									if ( parameter.intValue > condision.intValue )
									{
										result++;
									}
									break;
								case Condision.Type.Less:
									if ( parameter.intValue < condision.intValue )
									{
										result++;
									}
									break;
								case Condision.Type.Equals:
									if ( parameter.intValue == condision.intValue )
									{
										result++;
									}
									break;
								case Condision.Type.NotEquals:
									if ( parameter.intValue != condision.intValue )
									{
										result++;
									}
									break;
							}
						}
						break;
					case Parameter.Type.Float:
						{
							switch (condision.type)
							{
								case Condision.Type.Greater:
									if ( parameter.floatValue >= condision.floatValue )
									{
										result++;
									}
									break;
								case Condision.Type.Less:
									if ( parameter.floatValue <= condision.floatValue )
									{
										result++;
									}
									break;
								case Condision.Type.Equals:
									if ( parameter.floatValue == condision.floatValue )
									{
										result++;
									}
									break;
								case Condision.Type.NotEquals:
									if ( parameter.floatValue != condision.floatValue )
									{
										result++;
									}
									break;
							}
						}
						break;
					case Parameter.Type.Bool:
						{
							if ( parameter.boolValue == condision.boolValue )
							{
								result++;
							}
						}
						break;
					case Parameter.Type.GameObject:
						{
							if (parameter.gameObjectValue == condision.gameObjectValue)
							{
								result++;
							}
						}
						break;
				}
			}

			return count > 0 && count == result;
		}
	}
}

