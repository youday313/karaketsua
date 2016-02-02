using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Animator/CalcAnimatorParameter")]
	[BuiltInBehaviour]
	public class CalcAnimatorParameter : StateBehaviour, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("reference")]
		[SerializeField]
		private AnimatorParameterReference _Reference;

		public enum Function
		{
			Assign,
			Add,
		}

		[FormerlySerializedAs("function")]
		[SerializeField]
		private Function _Function;

		[FormerlySerializedAs("floatValue")]
		[SerializeField]
		private float _OldFloatValue;

		[FormerlySerializedAs("intValue")]
		[SerializeField]
		private int _OldIntValue;

		[FormerlySerializedAs("boolValue")]
		[SerializeField]
		private bool _OldBoolValue;

		[SerializeField]
		private int _SerializeVersion;
		[SerializeField]
		private FlexibleFloat _FloatValue;
		[SerializeField]
		private FlexibleInt _IntValue;
		[SerializeField]
		private FlexibleBool _BoolValue;

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

		private int _ParameterID;

		void Awake()
		{
			_ParameterID = Animator.StringToHash(_Reference.name);
        }

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Reference.animator == null)
			{
				return;
			}

			switch (_Reference.type)
			{
				case 1:// Float
					{
						float value = _Reference.animator.GetFloat(_ParameterID);
						switch (_Function)
						{
							case Function.Assign:
								value = floatValue;
								break;
							case Function.Add:
								value += floatValue;
								break;
						}
						_Reference.animator.SetFloat(_ParameterID, value);
					}
					break;
				case 3:// Int
					{
						int value = _Reference.animator.GetInteger(_ParameterID);
						switch (_Function)
						{
							case Function.Assign:
								value = intValue;
								break;
							case Function.Add:
								value += intValue;
								break;
						}
						_Reference.animator.SetInteger(_ParameterID, value);
					}
					break;
				case 4:// Bool
					{
						_Reference.animator.SetBool(_ParameterID, boolValue);
					}
					break;
				case 9:// Trigger
					{
						_Reference.animator.SetTrigger(_ParameterID);
					}
					break;
			}
		}
	}
}
