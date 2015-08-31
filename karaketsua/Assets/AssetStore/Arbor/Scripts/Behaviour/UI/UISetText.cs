using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetText")]
	public class UISetText : StateBehaviour
	{
		[SerializeField] private Text _Text;
		[Multiline(3)][SerializeField] private string _String;

		void Awake()
		{
			if (_Text == null)
			{
				_Text = GetComponent<Text>();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Text != null)
			{
				_Text.text = _String;
			}
		}
	}
}