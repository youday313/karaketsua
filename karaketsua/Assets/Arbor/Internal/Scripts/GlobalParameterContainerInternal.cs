using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// シーンをまたいでもアクセス可能なParameterContainerrを扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class dealing with the accessible ParameterContainer even across the scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class GlobalParameterContainerInternal : ParameterContainerBase
	{
		[SerializeField] private ParameterContainerInternal _Prefab;

#if ARBOR_DOC_JA
		/// <summary>
		/// 元のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		///It returns the original ParameterContainer.
		/// </summary>
#endif
		public ParameterContainerInternal prefab
		{
			get
			{
				return _Prefab;
			}
		}

		static Dictionary<ParameterContainerInternal, ParameterContainerInternal> _Instancies = new Dictionary<ParameterContainerInternal, ParameterContainerInternal>();

		private ParameterContainerInternal _Instance;

#if ARBOR_DOC_JA
		/// <summary>
		/// 実体のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		/// It returns the ParameterContainer entity.
		/// </summary>
#endif
		public ParameterContainerInternal instance
		{
			get
			{
				MakeInstance();
                return _Instance;
			}
		}

		void MakeInstance()
		{
			if (_Prefab != null && _Instance == null)
			{
				if (!_Instancies.TryGetValue(_Prefab, out _Instance))
				{
					_Instance = (ParameterContainerInternal)Instantiate(_Prefab);
					DontDestroyOnLoad(_Instance.gameObject);
					_Instancies.Add(_Prefab, _Instance);
				}
			}
		}

		void Awake()
		{
			MakeInstance();
		}
	}
}
