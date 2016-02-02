using UnityEngine;
using System.Collections;
using Arbor;

//攻防、技、実行選択のインターフェイス
public class UIBottomButtonSetActiveCommand : UIBottomButtonBackState
{
    public GameObject actionParent;
    public GameObject wazaParent;
    public GameObject executeAttackParent;
    public GameObject executeDeffenceParent;

	// Use this for initialization
	void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        base.OnStateBegin();
        actionParent.SetActive(false);
        wazaParent.SetActive(false);
        executeAttackParent.SetActive(false);
        executeAttackParent.SetActive(false);
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
