using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenTextureOffset")]
	[BuiltInBehaviour]
	public class TweenTextureOffset : TweenBase
	{
		[SerializeField] private Renderer _Target;
		[SerializeField] private string _PropertyName = "_MainTex";
		[SerializeField] private bool _Relative;
        [SerializeField] private Vector2 _From = Vector2.zero;
		[SerializeField] private Vector2 _To = Vector2.zero;

		void Awake()
		{
			if (_Target == null)
			{
				_Target = GetComponent<Renderer>();
			}
		}

		Vector2 _StartOffset;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			if (_Relative)
			{
				_StartOffset = _Target.sharedMaterial.GetTextureOffset(_PropertyName);
			}
			else
			{
				_StartOffset = Vector2.zero;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_Target != null)
			{
				_Target.sharedMaterial.SetTextureOffset(_PropertyName , _StartOffset + Vector2.Lerp(_From, _To, factor) );
			}
		}
	}
}