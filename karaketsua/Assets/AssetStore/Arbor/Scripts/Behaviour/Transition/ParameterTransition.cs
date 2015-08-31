using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("ParameterTransition")]
	[AddBehaviourMenu("Transition/ParameterTransition")]
	[BuiltInBehaviour]
	public class ParameterTransition : StateBehaviour
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

			public ParameterReference reference;

			public Type type;

			public int intValue;
			public int floatValue;
			public bool boolValue;
		}

		[SerializeField] private StateLink _NextState;
		[SerializeField] private List<Condision> _Condisions = new List<Condision>();

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
				}
			}

			return count > 0 && count == result;
		}
	}
}

