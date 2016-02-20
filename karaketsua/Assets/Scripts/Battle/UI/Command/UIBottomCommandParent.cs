using UnityEngine;
using System.Collections;
using System;

using BattleScene;

namespace BattleScene
{
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

        //public UIBottomBack backButton;
        public event Action UpdateCommandUI=null;
        public event Action UpdateCameraUIMode=null;

        // Use this for initialization
        void Start()
        {
            BSceneState.Instance.StartWave += StartWave;
        }
        public void StartWave()
        {
            CreateAction();
        }

        //public void UpdateUI()
        //{
        //    actionScript.UpdateUI();
        //    wazaSelectScript.UpdateUI();
        //    executeAttackScript.UpdateUI();
        //    executeDeffenceScript.UpdateUI();
        //}



        public void CreateAction()
        {
            Off();
            actionParent.SetActive(true);
            actionScript.UpdateUI();
            UpdateCameraUIMode();
            BCameraChange.Instance.ActiveLeanMode();
        }

        public void CreateWazaSelect()
        {
            Off();
            wazaSelectParent.SetActive(true);
            wazaSelectScript.UpdateUI();
            //UpdateCommandUI();
            UpdateCameraUIMode();
            BCameraChange.Instance.ActiveLeanMode();
        }
        public void CreateExecuteAttack()
        {
            Off();
            executeAttackParent.SetActive(true);
            executeAttackScript.UpdateUI();
            //UpdateCommandUI();
            UpdateCameraUIMode();
            BCameraChange.Instance.ActiveUpMode();
        }
        public void CreateExecuteDeffence()
        {
            Off();
            executeDeffenceParent.SetActive(true);
            executeDeffenceScript.UpdateUI();
            //UpdateCommandUI();
            UpdateCameraUIMode();
            BCameraChange.Instance.ActiveLeanMode();
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

        //public void OnBack()
        //{
        //    if (enableCommand == EnableCommandUIState.Waza)
        //    {
        //        CreateAction();
        //    }
        //    else if (enableCommand == EnableCommandUIState.ExecuteAttack)
        //    {
        //        CreateWazaSelect();

        //    }
        //    else if (enableCommand == EnableCommandUIState.ExecuteDeffence)
        //    {
        //        CreateAction();
        //    }
        //}
    }
}