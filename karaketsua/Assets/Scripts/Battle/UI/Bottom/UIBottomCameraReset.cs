using UnityEngine;
using System.Collections;

public class UIBottomCameraReset : UIBottomBase {
    public UIBottomCameraParent cameraParent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClick()
    {


        cameraParent.Reset();
    }
}
