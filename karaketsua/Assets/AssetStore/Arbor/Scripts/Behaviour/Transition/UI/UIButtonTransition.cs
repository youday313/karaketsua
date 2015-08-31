using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UIButtonTransition")]
	[BuiltInBehaviour]
	public class UIButtonTransition : StateBehaviour
	{
		[SerializeField] private Button _Button;
		[SerializeField] private StateLink _NextState;

		void Awake()
		{
			if (_Button == null)
			{
				_Button = GetComponent<Button>();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Button != null)
			{
				_Button.onClick.AddListener(OnClick);
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_Button != null)
			{
				_Button.onClick.RemoveListener(OnClick);
			}
		}

		public void OnClick()
		{
			Transition(_NextState);
		}
	}
}