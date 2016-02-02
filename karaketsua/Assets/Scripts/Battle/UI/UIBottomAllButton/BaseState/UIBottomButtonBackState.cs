using UnityEngine;
using System.Collections;
using Arbor;
using UnityEngine.UI;

public class UIBottomButtonBackState : UIBottomButtonCameraModeState {
    public Button backButton;
    public bool IsEnableBack=false;
	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        backButton.interactable = IsEnableBack;

	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
