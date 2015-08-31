using UnityEngine;
using System.Collections;

using Arbor;

public class LookAt : MonoBehaviour
{
	public Transform target;

	private Transform _Transform;

	void Awake()
	{
		_Transform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if (target != null)
		{
			_Transform.LookAt(target);
        }	
	}
}
