using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateBehaviourの表示するタイトルを指定する属性。
	/// </summary>
#else	
	/// <summary>
	/// Attribute that specifies the title to display the StateBehaviour.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
	public sealed class BehaviourTitle : Attribute
	{
		private string _TitleName;
		
		public string titleName
		{
			get
			{
				return _TitleName;
			}
		}
		
		public BehaviourTitle (string titleName)
		{
			_TitleName = titleName;
		}
	}
}