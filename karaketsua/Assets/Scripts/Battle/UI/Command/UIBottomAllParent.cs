using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    //UIのオンオフ
    public class UIBottomAllParent : SingletonMonoBehaviour<UIBottomAllParent>
    {

        GameObject commandParent;
        public UIBottomCommandParent commandScript;

        

        // Use this for initialization
        void Start()
        {
            commandParent = commandScript.gameObject;
            BCharacterBase.OnActiveStaticE+=UpdateUI;
            BCharacterBase.OnEndActiveStaticE += UpdateUI;
            Off();
            
            StartWave();
        }

        public void StartWave()
        {
            UpdateUI();
        }

        public void UpdateUI(BCharacterBase chara)
        {
            commandParent.SetActive(true);
            commandScript.UpdateUI();
        }
        public void UpdateUI()
        {
            UpdateUI(null);
        }

        public void Off()
        {
            commandParent.SetActive(false);
        }


    }
}