using UnityEngine;
using System.Collections;
using Arbor;

public class UIBottomButtonSelectWaza : UIBottomButtonSetActiveCommand {
	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        wazaParent.SetActive(true);
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
