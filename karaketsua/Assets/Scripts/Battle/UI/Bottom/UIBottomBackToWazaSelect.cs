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
            CharacterManager.Instance.GetActiveCharacter().SelectDisable();
        }
    }
}