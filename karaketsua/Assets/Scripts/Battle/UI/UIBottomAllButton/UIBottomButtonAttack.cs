using UnityEngine;
using System.Collections;
using Arbor;
using UnityEngine.UI;

public class UIBottomButtonAttack : UIBottomButtonSetActiveCommand
{


    public Button attack;
    public bool enableAttack=false;
    public Button deffence;
    public bool enabledeffence=false;

	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        actionParent.SetActive(true);
        attack.interactable = enableAttack;
        attack.interactable = enableAttack;
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
