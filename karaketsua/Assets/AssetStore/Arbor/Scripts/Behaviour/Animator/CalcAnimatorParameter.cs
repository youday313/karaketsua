using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("CalcAnimatorParameter")]
	[AddBehaviourMenu("Animator/CalcAnimatorParameter")]
	[BuiltInBehaviour]
	public class CalcAnimatorParameter : StateBehaviour
	{
		public AnimatorParameterReference reference;

		public enum Function
		{
			Assign,
			Add,
		}
		public Function function;

		public int floatValue;
		public int intValue;
		public bool boolValue;

		private int _ParameterID;

		void Awake()
		{
			_ParameterID = Animator.StringToHash(reference.name);
        }

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (reference.animator == null)
			{
				return;
			}

			switch (reference.type)
			{
				case 1:// Float
					{
						float value = reference.animator.GetFloat(_ParameterID);
						switch (function)
						{
							case Function.Assign:
								value = floatValue;
								break;
							case Function.Add:
								value += floatValue;
								break;
						}
						reference.animator.SetFloat(_ParameterID, value);
					}
					break;
				case 3:// Int
					{
						int value = reference.animator.GetInteger(_ParameterID);
						switch (function)
						{
							case Function.Assign:
								value = intValue;
								break;
							case Function.Add:
								value += intValue;
								break;
						}
						reference.animator.SetInteger(_ParameterID, value);
					}
					break;
				case 4:// Bool
					{
						reference.animator.SetBool(_ParameterID, boolValue);
					}
					break;
				case 9:// Trigger
					{
						reference.animator.SetTrigger(_ParameterID);
					}
					break;
			}
		}
	}
}