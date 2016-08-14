using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteDeffence : UIBottomBase
    {
        public void OnClick()
        {
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllManager.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.ExecuteDeffence();
        }
    }
}