using UnityEngine;
using System.Collections;

public class UIBottomSpecialWaza : UIBottomBase {

    public UIBottomCommandParent commandParent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateUI()
    {
        base.UpdateUI();
    }
    public void OnClick()
    {
        commandParent.CreateExecuteAttack();
    }
}
