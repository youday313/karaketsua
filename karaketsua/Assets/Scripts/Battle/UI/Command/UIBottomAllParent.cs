using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    //UIのオンオフ
    public class UIBottomAllParent : Singleton<UIBottomAllParent>
    {

        GameObject commandParent;
        public UIBottomCommandParent commandScript;

        

        // Use this for initialization
        void Start()
        {
            commandParent = commandScript.gameObject;
            BCharacterBase.OnActiveStaticE+=UpdateUI;

            
            //BSceneState.Instance.StartWave += StartWave;
        }

        public void StartWave()
        {
            UpdateUI();
        }

        public void UpdateUI(BCharacterBase chara=null)
        {
            commandParent.SetActive(true);
            commandScript.UpdateUI();
        }

        public void Off()
        {
            commandParent.SetActive(false);
        }


    }
}