using UnityEngine;
using System.Collections;
using Arbor;
using UnityEngine.UI;

//カメラ操作があるかないか
public class UIBottomButtonCameraModeState : UIBottomButtonAllOnState
{
    public bool isCameraResetMode=false;
    public bool isEnableCamera=false;
    public GameObject cameraChangeButton;
    public GameObject cameraResetButton;
    // Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        cameraChangeButton.SetActive(!isCameraResetMode);
        cameraChangeButton.GetComponent<Button>().interactable = isEnableCamera;
        cameraResetButton.SetActive(isCameraResetMode);
        
     
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
