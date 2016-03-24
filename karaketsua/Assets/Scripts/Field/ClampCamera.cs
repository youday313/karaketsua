using UnityEngine;
using System.Collections;

public class ClampCamera : MonoBehaviour {

    public float x;

	// Use this for initialization
	void Start () {
	
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Clamp();
	}

    void Clamp()
    {
        CSTransform.SetX(transform,Mathf.Clamp(transform.position.x, -x, x));
    }
}
