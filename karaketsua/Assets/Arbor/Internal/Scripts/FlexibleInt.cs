using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なint型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible int type reference method there is more than one.
	/// </summary>
#endif

	[System.Serializable]
	public class FlexibleInt
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
		[SerializeField] private int _Value;
		[SerializeField] private IntParameterReference _Parameter;
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
		public int value
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
							return _Parameter.parameter.intValue;
						}
						return 0;
					case Type.Random:
						return Random.Range(_MinRange, _MaxRange);
				}

				return 0;
			}
		}

		public FlexibleInt(int value)
		{
			_Type = Type.Constant;
			_Value = value;
		}

		public FlexibleInt(IntParameterReference parameter)
		{
			_Type = Type.Parameter;
			_Parameter = parameter;
		}

		public FlexibleInt(int minRange,int maxRange)
		{
			_Type = Type.Random;
			_MinRange = minRange;
			_MaxRange = maxRange;
		}

		public static explicit operator int (FlexibleInt ai)
		{
			return ai.value;
		}

		public static explicit operator FlexibleInt(int value)
		{
			return new FlexibleInt(value);
		}
	}
}
