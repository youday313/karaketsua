using UnityEngine;
using System.Collections;
using Arbor;

public class BCameraBaseState : StateBehaviour {
	public GameObject nowCamera;
	public CameraTransform cameraTransform;
	// Use this for initialization
	void Start () {
		
	}

	// Use this for enter state
	public override void OnStateBegin() {
		nowCamera.SetActive (true);
	}

	// Use this for exit state
	public override void OnStateEnd() {
		nowCamera.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
public class CameraTransform{

	public Vector3 position;
	public Vector3 rotation;
}
