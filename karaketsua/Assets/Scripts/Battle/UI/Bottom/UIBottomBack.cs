using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using BattleScene;

namespace BattleScene
{
    public class UIBottomBack : UIBottomBase
    {

        [SerializeField]Button button;
        public event Action OnClickE;
        public UIBottomCommandParent command;
        // Use this for initialization

        void Awake()
        {
            button = GetComponent<Button>();
            button.interactable = false;
        }

        void Start()
        {
            //command.UpdateCommandUI += SetEnable;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateUI()
        {

            button.interactable = false;

        }

        //public void SetEnable()
        //{
        //    button.interactable = false;

        //    if (command.enableCommand == UIBottomCommandParent.EnableCommandUIState.Waza ||
        //        command.enableCommand == UIBottomCommandParent.EnableCommandUIState.ExecuteAttack ||
        //        command.enableCommand == UIBottomCommandParent.EnableCommandUIState.ExecuteDeffence)
        //    {
        //        button.interactable = true;
        //    }
        //}


        public void OnClick()
        {
            OnClickE();
        }
    }
}