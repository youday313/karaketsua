using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomBackToAction : UIBottomButton
    {
        public override void OnClick()
        {

            //BCharacterManager.Instance.ActiveCharacter.SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.Action;
            UIBottomAllManager.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.BackToActionSelect();
        }
    }
}