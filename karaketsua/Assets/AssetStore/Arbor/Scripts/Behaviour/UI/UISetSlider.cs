using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetSlider")]
	[BuiltInBehaviour]
	public class UISetSlider: StateBehaviour
	{
		[SerializeField] private Slider _Slider;
		[SerializeField] private float _Value;

		void Awake()
		{
			if (_Slider == null)
			{
				_Slider = GetComponent<Slider>();
			}
		}

		void UpdateSlider()
		{
			if (_Slider != null )
			{
				_Slider.value = _Value;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateSlider();
		}
	}
}