using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
[AddBehaviourMenu("Example/Menu")]
public class ExampleMenuTransition : StateBehaviour
{
	[Multiline(3)][SerializeField] private string _MenuName;

	[System.Serializable]
	public class Item
	{
		public string name;
		public StateLink link;
	}

	[SerializeField] private List<Item> _Items;

	void OnGUI()
	{
		GUILayout.Label( _MenuName );

		foreach( Item item in _Items )
		{
			if( GUILayout.Button( item.name ) )
			{
				Transition( item.link );
			}	
		}
	}
}
