using UnityEngine;
using System.Collections;
using System;

public class UIBottomCommandParent : UIBottomBase
{

     GameObject actionParent;
     GameObject wazaSelectParent;
     GameObject executeAttackParent;
     GameObject executeDeffenceParent;

    public UIBottomActionParent actionScript;
    public UIBottomWazaParent wazaSelectScript;
    public UIBottomExecuteAttackParent executeAttackScript;
    public UIBottomExecuteDeffenceParent executeDeffenceScript;

    public UIBottomBack backButton;
    public event Action UpdateCommandUI;
    public event Action UpdateCameraUIMode;

    public enum EnableCommandUIState
    {
        None,
        Action,
        Waza,
        ExecuteAttack,
        ExecuteDeffence,
    }
    public EnableCommandUIState enableCommand=EnableCommandUIState.None;
	// Use this for initialization
	void Start () {
        actionParent = actionScript.gameObject;
        wazaSelectParent = wazaSelectScript.gameObject;
        executeAttackParent = executeAttackParent.gameObject;
        executeDeffenceParent = executeDeffenceParent.gameObject;
        backButton.OnClickE += OnBack;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateUI()
    {
        //一度オフ
        Off();
        //処理
        if (enableCommand == EnableCommandUIState.None)
        {
            CreateAction();
        }
    }

    public void CreateAction()
    {
        Off();
        actionParent.SetActive(true);
        actionScript.UpdateUI();
        enableCommand = EnableCommandUIState.Action;
        UpdateCameraUIMode();
    }

    public void CreateWazaSelect()
    {
        Off();
        wazaSelectParent.SetActive(true);
        wazaSelectScript.UpdateUI();
        enableCommand = EnableCommandUIState.Waza;
        UpdateCommandUI();
        UpdateCameraUIMode();
    }
    public void CreateExecuteAttack()
    {
        Off();
        executeAttackParent.SetActive(true);
        executeAttackScript.UpdateUI();
        enableCommand = EnableCommandUIState.ExecuteAttack;
        UpdateCommandUI();
        UpdateCameraUIMode();
    }
    public void CreateExecuteDeffence()
    {
        Off();
        executeDeffenceParent.SetActive(true);
        executeDeffenceScript.UpdateUI();
        enableCommand = EnableCommandUIState.ExecuteDeffence;
        UpdateCommandUI();
        UpdateCameraUIMode();
    }

    void Off()
    {
        actionParent.SetActive(false);
        wazaSelectParent.SetActive(false);
        executeAttackParent.SetActive(false);
        executeDeffenceParent.SetActive(false);
        enableCommand = EnableCommandUIState.None;
        UpdateCommandUI();
        UpdateCameraUIMode();
    }

    public void OnBack()
    {
        if (enableCommand == EnableCommandUIState.Waza)
        {
            CreateAction();
        }
        else if (enableCommand == EnableCommandUIState.ExecuteAttack)
        {
            CreateWazaSelect();
        }
        else if (enableCommand == EnableCommandUIState.ExecuteDeffence)
        {
            CreateAction();
        }
    }
}
