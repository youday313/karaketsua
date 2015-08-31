using UnityEngine;
using System.Collections;

public class ExampleDestroyArea : MonoBehaviour 
{
	void OnTriggerEnter( Collider collider )
	{
		GameObject.Destroy ( collider.gameObject );
	}
}
