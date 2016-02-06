using UnityEngine;
using System.Collections;

public class UIBottomWazaParent : UIBottomBase
{

    public UIBottomBase[] wazas;
    public UIBottomBase special;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateUI()
    {
        foreach(var waza in wazas){
            waza.UpdateUI();
        }
        special.UpdateUI();
    }
}
