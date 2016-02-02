using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterContainerに格納されるParameterのクラス。
	/// </summary>
#else
	/// <summary>
	/// Class of Parameter to be stored in the ParameterContainer.
	/// </summary>
#endif
	[System.Serializable]
	public class Parameter
	{
		public delegate void DelegateOnChanged(Parameter parameter);

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型。
		/// </summary>
#else
		/// <summary>
		/// Parameter type.
		/// </summary>
#endif
		public enum Type
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// Int型。
			/// </summary>
#else
			/// <summary>
			/// Int type.
			/// </summary>
#endif
			Int,

#if ARBOR_DOC_JA
			/// <summary>
			/// Float型。
			/// </summary>
#else
			/// <summary>
			/// Float type.
			/// </summary>
#endif
			Float,

#if ARBOR_DOC_JA
			/// <summary>
			/// Bool型。
			/// </summary>
#else
			/// <summary>
			/// Bool type.
			/// </summary>
#endif
			Bool,

#if ARBOR_DOC_JA
			/// <summary>
			/// GameObject型。
			/// </summary>
#else
			/// <summary>
			/// GameObject type.
			/// </summary>
#endif
			GameObject,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このパラメータが格納されているコンテナ。
		/// </summary>
#else
		/// <summary>
		/// Container this parameter is stored.
		/// </summary>
#endif
		public ParameterContainerInternal container;

#if ARBOR_DOC_JA
		/// <summary>
		/// ID。
		/// </summary>
#else
		/// <summary>
		/// ID.
		/// </summary>
#endif
		public int id;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型。
		/// </summary>
#else
		/// <summary>
		/// Type.
		/// </summary>
#endif
		public Type type;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの名前。
		/// </summary>
#else
		/// <summary>
		/// Name.
		/// </summary>
#endif
		public string name;

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値。変更した後はOnChanged()を呼び出すこと。
		/// </summary>
#else
		/// <summary>
		/// Int value of type. And invoking the OnChanged () after changing.
		/// </summary>
#endif
		public int intValue;

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値。変更した後はOnChanged()を呼び出すこと。
		/// </summary>
#else
		/// <summary>
		/// Float value of type. And invoking the OnChanged () after changing.
		/// </summary>
#endif
		public float floatValue;

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値。変更した後はOnChanged()を呼び出すこと。
		/// </summary>
#else
		/// <summary>
		/// Bool value of type. And invoking the OnChanged () after changing.
		/// </summary>
#endif
		public bool boolValue;

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObject型の値。変更した後はOnChanged()を呼び出すこと。
		/// </summary>
#else
		/// <summary>
		/// GameObject value of type. And invoking the OnChanged () after changing.
		/// </summary>
#endif
		public GameObject gameObjectValue;

		public object value
		{
			get
			{
				switch (type)
				{
					case Type.Int:
						return intValue;
					case Type.Float:
						return floatValue;
					case Type.Bool:
						return boolValue;
					case Type.GameObject:
						return gameObjectValue;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値が変更された際に呼び出されるコールバック関数。
		/// </summary>
#else
		/// <summary>
		/// Callback function to be called when the value is changed.
		/// </summary>
#endif
		public event DelegateOnChanged onChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を変更した際に呼び出す。
		/// </summary>
#else
		/// <summary>
		/// Call when you change the value.
		/// </summary>
#endif
		public void OnChanged()
		{
			if( onChanged != null )
			{
				onChanged(this);
			}
		}
    }
}
