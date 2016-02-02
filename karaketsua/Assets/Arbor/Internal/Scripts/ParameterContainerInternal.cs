using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータコンテナ。
	/// GameObjectにアタッチして使用する。
	/// </summary>
#else
	/// <summary>
	/// ParameterContainer.
	/// Is used by attaching to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class ParameterContainerInternal : ParameterContainerBase
	{
		[SerializeField]
		private List<Parameter> _Parameters = new List<Parameter>();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの配列を取得。
		/// </summary>
#else
		/// <summary>
		/// Get an array of parameters.
		/// </summary>
#endif
		public Parameter[] parameters
		{
			get
			{
				return _Parameters.ToArray();
			}
		}

		private Dictionary<int, Parameter> _DicParameters;

		private Dictionary<int, Parameter> dicParameters
		{
			get
			{
				if (_DicParameters == null)
				{
					_DicParameters = new Dictionary<int, Parameter>();

					foreach (Parameter param in _Parameters)
					{
						_DicParameters.Add(param.id, param);
					}
				}

				return _DicParameters;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 重複しない名前を生成する。
		/// </summary>
		/// <param name="name">元の名前。</param>
		/// <returns>結果の名前。</returns>
#else
		/// <summary>
		/// It generates a name that does not overlap.
		/// </summary>
		/// <param name="name">The original name.</param>
		/// <returns>Result.</returns>
#endif
		public string MakeUniqueName(string name)
		{
			string searchName = name;
			int count = 0;
			while (true)
			{
				if (GetParam(searchName) == null)
				{
					break;
				}

				searchName = name + " " + count;
				count++;
			}

			return searchName;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Float type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetFloat(string name, float value)
		{
			Parameter parameter = GetParam(name);
			if (parameter != null && parameter.type == Parameter.Type.Float)
			{
				if (parameter.floatValue != value)
				{
					parameter.floatValue = value;
					parameter.OnChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Float type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool GetFloat(string name, out float value)
		{
			Parameter parameter = GetParam(name);
			if (parameter == null || parameter.type != Parameter.Type.Float)
			{
				value = 0.0f;
				return false;
			}

			value = parameter.floatValue;

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetInt(string name, int value)
		{
			Parameter parameter = GetParam(name);
			if (parameter != null && parameter.type == Parameter.Type.Int)
			{
				if (parameter.intValue != value )
				{
					parameter.intValue = value;
					parameter.OnChanged();
                }
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool GetInt(string name, out int value)
		{
			Parameter parameter = GetParam(name);
			if (parameter == null || parameter.type != Parameter.Type.Int)
			{
				value = 0;
				return false;
			}

			value = parameter.intValue;

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Bool type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetBool(string name, bool value)
		{
			Parameter parameter = GetParam(name);
			if (parameter != null && parameter.type == Parameter.Type.Bool)
			{
				if (parameter.boolValue != value)
				{
					parameter.boolValue = value;
					parameter.OnChanged();
                }
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Bool type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool GetBool(string name, out bool value)
		{
			Parameter parameter = GetParam(name);
			if (parameter == null || parameter.type != Parameter.Type.Bool)
			{
				value = false;
				return false;
			}

			value = parameter.boolValue;

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを追加する。
		/// </summary>
		/// <param name="name">名前。nameが重複していた場合はユニークな名前に変更される。</param>
		/// <param name="type">型。</param>
		/// <returns>追加されたパラメータ。</returns>
#else
		/// <summary>
		/// Add a parameter.
		/// </summary>
		/// <param name="name">Name. It is changed to a unique name if the name had been duplicated.</param>
		/// <param name="type">Type.</param>
		/// <returns>It added parameters.</returns>
#endif
		public Parameter AddParam(string name, Parameter.Type type)
		{
			Parameter parameter = new Parameter();
			parameter.container = this;
			parameter.id = MakeUniqueParamID();
			parameter.name = MakeUniqueName(name);
			parameter.type = type;
			
			_Parameters.Add(parameter);
			dicParameters.Add(parameter.id, parameter);

			return parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前からパラメータを取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータ。存在しなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the parameters from the name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Parameters. Return null if you did not exist.</returns>
#endif
		public Parameter GetParam(string name)
		{
			return _Parameters.Find(parameter => parameter.name == name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IDからパラメータを取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータ。存在しなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the parameters from the ID.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>Parameters. Return null if you did not exist.</returns>
#endif
		public Parameter GetParam(int id)
		{
			Parameter result = null;
			if (dicParameters.TryGetValue(id, out result))
			{
				if (result.id == id)
				{
					return result;
				}
			}

			foreach (Parameter parameter in _Parameters)
			{
				if (parameter.id == id)
				{
					dicParameters.Add(parameter.id, parameter);
					return parameter;
				}
			}

			return null;
		}

		int MakeUniqueParamID()
		{
			int count = _Parameters.Count;

			System.Random random = new System.Random(count);

			while (true)
			{
				int id = random.Next();

				if (id != 0 && GetParam(id) == null)
				{
					return id;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前を指定してパラメータを削除する。
		/// </summary>
		/// <param name="name">名前。</param>
#else
		/// <summary>
		/// Delete the parameters by name.
		/// </summary>
		/// <param name="name">Name.</param>
#endif
		public void DeleteParam(string name)
		{
			Parameter parameter = GetParam(name);
			if (parameter != null)
			{
				_Parameters.Remove(parameter);
				dicParameters.Remove(parameter.id);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IDを指定してパラメータを削除する。
		/// </summary>
		/// <param name="id">ID。</param>
#else
		/// <summary>
		/// Delete the parameters by ID.
		/// </summary>
		/// <param name="id">ID.</param>
#endif
		public void DeleteParam(int id)
		{
			Parameter parameter = GetParam(id);
			if (parameter != null)
			{
				_Parameters.Remove(parameter);
				dicParameters.Remove(parameter.id);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを削除する。
		/// </summary>
		/// <param name="parameter">パラメータ。</param>
#else
		/// <summary>
		/// Delete a parameter.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
#endif
		public void DeleteParam(Parameter parameter)
		{
			if (parameter != null)
			{
				_Parameters.Remove(parameter);
				dicParameters.Remove(parameter.id);
			}
		}
	}
}
