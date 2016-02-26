using UnityEngine;
using System.Collections;
using System;

using BattleScene;

namespace BattleScene
{

    public enum EUICommandState
    {
        None,
        Action,
        Waza,
        ExecuteAttack,
        ExecuteDeffence
    }
    public class UIBottomCommandParent : Singleton<UIBottomCommandParent>
    {

        public GameObject actionParent;
        public GameObject wazaSelectParent;
        public GameObject executeAttackParent;
        public GameObject executeDeffenceParent;

        public UIBottomActionParent actionScript;
        public UIBottomWazaParent wazaSelectScript;
        public UIBottomExecuteAttackParent executeAttackScript;
        public UIBottomExecuteDeffenceParent executeDeffenceScript;

        public static EUICommandState UICommandState=EUICommandState.None;


        //public UIBottomBack backButton;
        public event Action UpdateCommandUI=null;
        public event Action UpdateCameraUIMode=null;

        // Use this for initialization
        void Start()
        {
            //BSceneState.Instance.StartWave += StartWave;
            //BCharacterBase.OnActiveStaticE += CreateAction;

        }

        public void UpdateUI()
        {
            Off();
            switch (UICommandState)
            {
                case EUICommandState.Action:
                    actionParent.SetActive(true);
                    actionScript.UpdateUI();
                    break;
                case EUICommandState.Waza:
                    wazaSelectParent.SetActive(true);
                    wazaSelectScript.UpdateUI();
                    break;
                case EUICommandState.ExecuteAttack:
                    executeAttackParent.SetActive(true);
                    executeAttackScript.UpdateUI();
                    break;
                case EUICommandState.ExecuteDeffence:
                    executeDeffenceParent.SetActive(true);
                    executeDeffenceScript.UpdateUI();
                    break;
            }
        }

        void Off()
        {
            actionParent.SetActive(false);
            wazaSelectParent.SetActive(false);
            executeAttackParent.SetActive(false);
            executeDeffenceParent.SetActive(false);
            //UpdateCommandUI();
            UpdateCameraUIMode();
        }
    }
}