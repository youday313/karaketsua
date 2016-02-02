using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateLinkが即時遷移フラグを固定した状態であることを設定。
	/// この指定とは別にTransiionメソッドのimmediate引数も指定すること。
	/// </summary>
#else
	/// <summary>
	/// Setting the StateLink is in a state of fixing an immediate transition flags.
	/// This specified separately that it also specify the immediate argument of Transiion method with.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class FixedImmediateTransition : Attribute
	{
		private bool _Immediate;
		public bool immediate
		{
			get
			{
				return _Immediate;
			}
		}

		public FixedImmediateTransition(bool immediate)
		{
			_Immediate = immediate;
		}
	}
}
