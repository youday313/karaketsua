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
            BSceneState.Instance.StartWave += StartWave;
        }

        public void StartWave()
        {
            CreateAction();
        }


        //public void UpdateUI()
        //{
        //    //bottomParent.SetActive(true);
        //    commandParent.SetActive(true);

        //    //bottomScript.UpdateUI();
        //    commandScript.UpdateUI();
        //}
        public void CreateAction()
        {
            //bottomParent.SetActive(true);
            commandParent.SetActive(true);

            //bottomScript.UpdateUI();
            commandScript.CreateAction();
        }

        public void Off()
        {
            //bottomParent.SetActive(false);
            commandParent.SetActive(false);
        }


    }
}