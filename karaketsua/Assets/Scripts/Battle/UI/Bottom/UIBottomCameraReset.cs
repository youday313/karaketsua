using UnityEngine;
using System.Collections;
using BattleScene;
using UnityEngine.UI;

namespace BattleScene
{
    public class UIBottomCameraReset : UIBottomBase
    {
        [SerializeField]Button button;
        public UIBottomCameraParent cameraParent;
        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            //カメラを動かしている
            if (BCameraMove.Instance.IsMoved == false)
            {
                button.interactable = false;
            }
            button.interactable = true;
        }

        public void OnClick()
        {
            BCameraMove.Instance.ResetCamera();
            cameraParent.Reset();
        }
    }
}