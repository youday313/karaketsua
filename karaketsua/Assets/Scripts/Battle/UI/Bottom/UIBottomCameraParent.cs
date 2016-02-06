using UnityEngine;
using System.Collections;

public class UIBottomCameraParent : UIBottomBase {

    public UIBottomCameraChange cameraChange;
    public UIBottomCameraReset cameraReset;
    GameObject cameraChangeObject;
    GameObject cameraResetObject;

    public UIBottomCommandParent commad;
	// Use this for initialization
	void Start () {
        cameraChangeObject = cameraChange.gameObject;
        cameraResetObject = cameraReset.gameObject;

        cameraChangeObject.SetActive(true);
        cameraResetObject.SetActive(false);

        commad.UpdateCameraUIMode += Reset;
	}
	
	// Update is called once per frame
	void Update () {
        cameraChangeObject.SetActive(true);
        cameraResetObject.SetActive(false);

        cameraChange.UpdateUI();
	}

    public void Reset()
    {
        cameraChangeObject.SetActive(true);
        cameraResetObject.SetActive(false);

        cameraChange.UpdateUI();
    }
    public void MoveCamera()
    {
        cameraChangeObject.SetActive(false);
        cameraResetObject.SetActive(true);

        cameraReset.UpdateUI();
    }


}
