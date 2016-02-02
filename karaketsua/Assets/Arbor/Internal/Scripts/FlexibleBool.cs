using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なbool型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible bool type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public class FlexibleBool
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

#if ARBOR_DOC_JA
			/// <summary>
			/// ランダム
			/// </summary>
#else
			/// <summary>
			/// Random
			/// </summary>
#endif
			Random,
		};

		[SerializeField] private Type _Type = Type.Constant;
		[SerializeField] private bool _Value;
		[SerializeField] private BoolParameterReference _Parameter;
		[Range(0.0f,1.0f),SerializeField] private float _Probability;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public bool value
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
							return _Parameter.parameter.boolValue;
						}
						return false;
					case Type.Random:
						return Random.Range(0.0f, 1.0f) <= _Probability;
				}

				return false;
			}
		}

		public FlexibleBool(bool value)
		{
			_Type = Type.Constant;
			_Value = value;
		}

		public FlexibleBool(BoolParameterReference parameter)
		{
			_Type = Type.Parameter;
			_Parameter = parameter;
		}

		public FlexibleBool(float probability)
		{
			_Type = Type.Random;
			_Probability = probability;
		}

		public static explicit operator bool (FlexibleBool ab)
		{
			return ab.value;
		}

		public static explicit operator FlexibleBool(bool value)
		{
			return new FlexibleBool(value);
		}
	}
}
