using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;
namespace BattleScene
{
    public class UIBottomAttack : UIBottomBase
    {
        [SerializeField]
        private Button button;

        public override void UpdateUI()
        {
            button.interactable = false;
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if (chara == null) return;
            if (chara.IsAttacked()==true || chara.IsEnemy==true||chara.IsNowAction()==true) return;
            button.interactable = true;
        }

        //ボタンクリック
        public void OnClick()
        {
            //BCharacterManager.Instance.ActiveCharacter.SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.Waza;
            UIBottomAllManager.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectAttack();
        }
    }
}