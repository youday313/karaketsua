using UnityEngine;
using System.Collections;

using BattleScene;

namespace BattleScene
{
    public class UIBottomBackToWazaSelect : UIBottomButton
    {
        public override void OnClick()
        {
            //BCharacterManager.Instance.ActiveCharacter.SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.Waza;
            UIBottomAllParent.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectAttack();

            //カメラが横から
            BCameraChange.Instance.ActiveLeanMode();

        }
    }
}