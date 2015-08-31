using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("CalcParameter")]
	[AddBehaviourMenu("Parameter/CalcParameter")]
	[BuiltInBehaviour]
	public class CalcParameter : StateBehaviour
	{
		public ParameterReference reference;

		public enum Function
		{
			Assign,
			Add,
		}
		public Function function;

		public int intValue;
		public int floatValue;
		public bool boolValue;

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
						bool value = parameter.boolValue;

						if (parameter.boolValue != value)
						{
							parameter.boolValue = value;
							parameter.OnChanged();
						}
					}
					break;
			}
		}
	}
}
