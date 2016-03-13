using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteDeffence : UIBottomBase
    {

        public UIBottomCommandParent commandParent;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick()
        {
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllParent.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.ExecuteDeffence();
        }
    }
}