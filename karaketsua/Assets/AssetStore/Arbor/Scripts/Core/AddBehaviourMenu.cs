using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AddBehaviourメニューでの名前を指定する属性。
	/// </summary>
#else	
	/// <summary>
	/// Attribute that specifies the name of at AddBehaviour menu.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
	public sealed class AddBehaviourMenu : System.Attribute
	{
		private string _MenuName;

		public string menuName
		{
			get
			{
				return _MenuName;
			}
		}
		
		public AddBehaviourMenu(string menuName)
		{
			_MenuName = menuName;
		}
	}
}