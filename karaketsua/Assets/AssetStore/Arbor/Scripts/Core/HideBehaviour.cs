using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AddBehaviourメニューに表示しないようにする属性。
	/// </summary>
#else	
	/// <summary>
	/// The attributes you do not want to display to AddBehaviour menu.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = false)]
	public sealed class HideBehaviour : System.Attribute
	{
		public HideBehaviour(){}
	}
}
