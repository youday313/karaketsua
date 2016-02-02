using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetTextFromParameter")]
	[BuiltInBehaviour]
	public class UISetTextFromParameter : StateBehaviour
	{
		[SerializeField]
		private Text _Text;
		[SerializeField]
		private ParameterReference _Parameter;
		[SerializeField]
		private string _Format;
		[SerializeField]
		private bool _ChangeTimingUpdate;

		void Awake()
		{
			if (_Text == null)
			{
				_Text = GetComponent<Text>();
			}
		}

		void UpdateText()
		{
			if (_Text != null && _Parameter.parameter != null)
			{
				switch (_Parameter.parameter.type)
				{
					case Parameter.Type.Int:
						_Text.text = _Parameter.parameter.intValue.ToString(_Format);
						break;
					case Parameter.Type.Float:
						_Text.text = _Parameter.parameter.floatValue.ToString(_Format);
						break;
					case Parameter.Type.Bool:
						_Text.text = _Parameter.parameter.boolValue.ToString();
						break;
					case Parameter.Type.GameObject:
						_Text.text = _Parameter.parameter.gameObjectValue.ToString();
						break;
				}
			}
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateText();
		}

		// Update is called once per frame
		public override void OnStateBegin()
		{
			UpdateText();

			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged += OnChangedParameter;
			}
		}

		public override void OnStateEnd()
		{
			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged -= OnChangedParameter;
			}
		}
	}
}
