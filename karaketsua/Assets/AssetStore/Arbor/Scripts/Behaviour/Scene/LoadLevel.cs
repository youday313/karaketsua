using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[BehaviourTitle("LoadLevel")]
	[AddBehaviourMenu("Scene/LoadLevel")]
	[BuiltInBehaviour]
	public class LoadLevel : StateBehaviour
	{
		[SerializeField] private string _LevelName;

		// Use this for enter state
		public override void OnStateBegin() 
		{
			Application.LoadLevel( _LevelName );
		}
	}
}