using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;
namespace BattleScene
{
    public class UIBottomDeffence : UIBottomBase
    {
        [SerializeField]
        private Button button;

        // UI情報更新
        public override void UpdateUI()
        {
            button.interactable = false;
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if (chara == null) return;
            if (chara.IsEnemy || chara.IsNowAction()) return;  
            button.interactable = true;
        }


        public void OnClick()
        {
            //BCharacterManager.Instance.ActiveCharacter.SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteDeffence;
            UIBottomAllManager.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectDeffence();
        }
    }
}
