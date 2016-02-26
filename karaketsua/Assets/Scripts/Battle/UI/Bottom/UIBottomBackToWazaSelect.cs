using UnityEngine;
using System.Collections;

using BattleScene;

namespace BattleScene
{
    public class UIBottomBackToWazaSelect : UIBottomButton
    {
        public override void OnClick()
        {
            UIBottomCommandParent.Instance.CreateWazaSelect();
            BCharacterManager.Instance.GetActiveCharacter().SelectDisable();
        }
    }
}