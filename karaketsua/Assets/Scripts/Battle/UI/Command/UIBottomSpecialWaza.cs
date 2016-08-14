using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{

    public class UIBottomSpecialWaza : UIBottomBase
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Text wazaName;


        public override void UpdateUI()
        {
            wazaName.text = "なし";
            button.interactable = false;

            //テキストの変更
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if (chara == null) return;

            var param = chara.characterParameter.moveAttackParameter;
            //技がある
            if (param!=null)
            {
                wazaName.text = param.wazaName;
                button.interactable = true;
            }
        }
        public void OnClick()
        {
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectMoveAttack();
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomAllManager.Instance.UpdateUI();
        }
    }
}