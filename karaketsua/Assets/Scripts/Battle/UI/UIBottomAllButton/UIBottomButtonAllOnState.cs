using UnityEngine;
using System.Collections;
using Arbor;

//UIをオン
//インターフェイス
public class UIBottomButtonAllOnState : UIBottomButtonBaseState {
	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        UIParent.SetActive(true);
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
