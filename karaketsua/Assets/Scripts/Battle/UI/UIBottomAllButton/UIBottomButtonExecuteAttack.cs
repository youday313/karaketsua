using UnityEngine;
using System.Collections;
using Arbor;
using UnityEngine.UI;

public class UIBottomButtonExecuteAttack : UIBottomButtonSetActiveCommand
{
    public StateLink offUI;
    public StateLink selectTarget;

    public Button execute;
    public bool isExecute = false;
	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        executeAttackParent.SetActive(true);
        execute.interactable = isExecute;

	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
