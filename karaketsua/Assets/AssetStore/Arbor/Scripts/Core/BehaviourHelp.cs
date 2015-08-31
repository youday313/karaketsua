using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateBehaviourのヘルプボタンから表示するURLを指定する属性。
	/// </summary>
#else	
	/// <summary>
	/// Attribute that specifies the URL to be displayed from the Help button of StateBehaviour.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
	public sealed class BehaviourHelp : Attribute
	{
		private string _Url;

		public string url
		{
			get
			{
				return _Url;
			}
		}

		public BehaviourHelp( string url )
		{
			_Url = url;
		}
	}
}