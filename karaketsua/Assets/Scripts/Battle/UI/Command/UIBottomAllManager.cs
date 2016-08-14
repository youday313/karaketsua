using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    //UIのオンオフ
    public class UIBottomAllManager: SingletonMonoBehaviour<UIBottomAllManager>
    {

        private GameObject commandParent;
        [SerializeField]
        private UIBottomCommandParent commandScript;

        // 初期化
        public void initialize()
        {
            commandParent = commandScript.gameObject;
            BCharacterBase.OnActiveStaticE += UpdateUI;
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