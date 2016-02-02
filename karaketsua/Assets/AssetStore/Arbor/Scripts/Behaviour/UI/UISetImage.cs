using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetImage")]
	public class UISetImage : StateBehaviour
	{
		[SerializeField] private Image _Image;
		[SerializeField] private Sprite _Sprite;

		void Awake()
		{
			if (_Image == null)
			{
				_Image = GetComponent<Image>();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Image != null)
			{
				_Image.sprite = _Sprite;
			}
		}
	}
}