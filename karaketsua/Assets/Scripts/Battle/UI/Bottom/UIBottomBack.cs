using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIBottomBack : UIBottomBase
{

    Button button;
    public event Action OnClickE;
    public UIBottomCommandParent commad;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        commad.UpdateCommandUI += SetEnable;
        button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UpdateUI()
    {
        button.interactable = false;
    }

    public void SetEnable()
    {
        button.interactable = false;

        if (commad.enableCommand == UIBottomCommandParent.EnableCommandUIState.Waza ||
            commad.enableCommand == UIBottomCommandParent.EnableCommandUIState.ExecuteAttack ||
            commad.enableCommand == UIBottomCommandParent.EnableCommandUIState.ExecuteDeffence)
        {
            button.interactable = true;
        }
    }

    public void OnClick()
    {
        OnClickE();
    }
}
