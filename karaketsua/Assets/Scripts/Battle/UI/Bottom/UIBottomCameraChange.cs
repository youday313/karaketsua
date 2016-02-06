using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBottomCameraChange : UIBottomBase {

    Button button;
    public UIBottomCameraParent parent;
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateUI()
    {
        button.interactable = false;
    }

    public void OnClick()
    {
        
    }

}
