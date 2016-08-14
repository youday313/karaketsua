using UnityEngine;
using System.Collections;

public class UIBottomWazaParent : UIBottomBase
{
    [SerializeField]
    private UIBottomBase[] wazas;
    [SerializeField]
    private UIBottomBase special;


    public override void UpdateUI()
    {
        
        foreach(var waza in wazas){
            waza.gameObject.SetActive(true);
            waza.UpdateUI();
        }
        special.gameObject.SetActive(true);
        special.UpdateUI();
    }
}
