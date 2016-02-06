using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBottomActionParent : UIBottomBase
{
    public UIBottomAttack attack;
    public UIBottomDeffence deffence;
	// Use this for initialization
	void Start () {
        
    }

	
	// Update is called once per frame
	void Update () {
	
	}
    public override void UpdateUI()
    {
        attack.UpdateUI();
        deffence.UpdateUI();
    }
}
