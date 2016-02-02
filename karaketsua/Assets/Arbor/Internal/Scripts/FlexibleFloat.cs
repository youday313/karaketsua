using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なfloat型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible float type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public class FlexibleFloat
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
		[SerializeField] private float _Value;
		[SerializeField] private FloatParameterReference _Parameter;
		[SerializeField] private float _MinRange;
		[SerializeField] private float _MaxRange;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public float value
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
							return _Parameter.parameter.floatValue;
						}
						return 0;
					case Type.Random:
						return Random.Range(_MinRange, _MaxRange);
				}

				return 0;
			}
		}

		public FlexibleFloat(float value)
		{
			_Type = Type.Constant;
			_Value = value;
		}

		public FlexibleFloat(FloatParameterReference parameter)
		{
			_Type = Type.Parameter;
			_Parameter = parameter;
		}

		public FlexibleFloat(float minRange, float maxRange)
		{
			_Type = Type.Random;
			_MinRange = minRange;
			_MaxRange = maxRange;
		}

		public static explicit operator float (FlexibleFloat af)
		{
			return af.value;
		}

		public static explicit operator FlexibleFloat(float value)
		{
			return new FlexibleFloat(value);
		}
	}
}
