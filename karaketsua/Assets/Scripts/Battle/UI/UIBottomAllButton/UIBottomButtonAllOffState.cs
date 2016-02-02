using UnityEngine;
using System.Collections;
using Arbor;

//UIをオフ
public class UIBottomButtonAllOffState : UIBottomButtonBaseState
{
    public StateLink inactiveState;
    public StateLink enableAttack;
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
