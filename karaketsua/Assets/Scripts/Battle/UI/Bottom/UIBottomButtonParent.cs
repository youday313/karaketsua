using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomButtonParent : UIBottomBase
    {

        public UIBottomBack back;
        public UIBottomCameraParent camera;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateUI()
        {
            back.UpdateUI();
            camera.UpdateUI();
        }
    }
}