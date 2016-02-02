using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なGameObject型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible GameObject type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public class FlexibleGameObject
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 参照タイプ
		/// </summary>
#else
		/// <summary>
		/// Reference type
		/// </summary>
#endif
		public enum Type
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 定数
			/// </summary>
#else
			/// <summary>
			/// Constant
			/// </summary>
#endif
			Constant,

#if ARBOR_DOC_JA
			/// <summary>
			/// パラメータ
			/// </summary>
#else
			/// <summary>
			/// Parameter
			/// </summary>
#endif
			Parameter,
		}
		[SerializeField] private Type _Type = Type.Constant;
		[SerializeField] private GameObject _Value;
		[SerializeField] private GameObjectParameterReference _Parameter;
		[SerializeField] private int _MinRange;
		[SerializeField] private int _MaxRange;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public GameObject value
		{
			get
			{
				switch (_Type)
				{
					case Type.Constant:
						return _Value;
					case Type.Parameter:
						if (_Parameter.parameter != null)
						{
							return _Parameter.parameter.gameObjectValue;
						}
						return null;
				}

				return null;
			}
		}

		public FlexibleGameObject(GameObject value)
		{
			_Type = Type.Constant;
			_Value = value;
		}

		public FlexibleGameObject(GameObjectParameterReference parameter)
		{
			_Type = Type.Parameter;
			_Parameter = parameter;
		}

		public static explicit operator GameObject (FlexibleGameObject ago)
		{
			return ago.value;
		}

		public static explicit operator FlexibleGameObject(GameObject value)
		{
			return new FlexibleGameObject(value);
		}
	}
}
