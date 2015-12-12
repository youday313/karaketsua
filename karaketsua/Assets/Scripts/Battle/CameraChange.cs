using UnityEngine;
using System.Collections;
using System;


public enum CameraMode { FromBack=0,FromFront,Up }
public class CameraChange : MonoBehaviour {

    public GameObject leanCameraObject;
    public GameObject upCameraObject;
    CameraMove leanCamera;
	// Use this for initialization
	void Start () {

        leanCamera = leanCameraObject.GetComponent<CameraMove>();

        leanCameraObject.SetActive(true);
        upCameraObject.SetActive(false);
    }
	
    public CameraMode nowCameraMode;
    public void ChangeCameraMode()
    {
        nowCameraMode = (CameraMode)(((int)nowCameraMode + 1) % Enum.GetNames(typeof(CameraMode)).Length);
        if (nowCameraMode == CameraMode.FromBack || nowCameraMode == CameraMode.FromFront)
        {

            ActiveLeanCamera(nowCameraMode);   
        }
        else
        {
            ActiveUpMode();
        }
        
    }
    void ActiveLeanCamera(CameraMode _cameraMode)
    {
        leanCameraObject.SetActive(true);
        upCameraObject.SetActive(false);

        leanCamera.ChangeFrontMode(_cameraMode);

        
    }
    void ActiveUpMode()
    {
        leanCameraObject.SetActive(false);
        upCameraObject.SetActive(true);
    }
}
