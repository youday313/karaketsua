using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BattleScene;

namespace BattleScene
{
    public class UIBottomCameraChange : UIBottomBase
    {

        Button button;
        public UIBottomCameraParent parent;
        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();
        }



        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateUI()
        {
            //カメラを動かしている
            if (BCameraMove.Instance.IsMoved == true)
            {
                button.interactable = false;
            }
            button.interactable = true;
        }

        public void OnClick()
        {

        }

    }
}