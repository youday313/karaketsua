using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/SetSprite")]
	[BuiltInBehaviour]
	public class SetSprite : StateBehaviour
	{
		[SerializeField] private SpriteRenderer _Target;
		[SerializeField] private Sprite _Sprite;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<SpriteRenderer>();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Target != null)
			{
				_Target.sprite = _Sprite;
			}
		}
	}
}