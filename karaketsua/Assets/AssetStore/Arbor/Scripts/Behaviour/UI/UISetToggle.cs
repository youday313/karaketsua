using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetToggle")]
	[BuiltInBehaviour]
	public class UISetToggle : StateBehaviour
	{
		[SerializeField] private Toggle _Toggle;
		[SerializeField] private bool _Value;

		void Awake()
		{
			if (_Toggle == null)
			{
				_Toggle = GetComponent<Toggle>();
			}
		}

		void UpdateToggle()
		{
			if (_Toggle != null )
			{
				_Toggle.isOn = _Value;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateToggle();
		}
	}
}