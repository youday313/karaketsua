using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteAttack : UIBottomBase
    {

        [SerializeField]
        Button button;

        // Update is called once per frame
        public override void UpdateUI()
        {
            button.interactable = false;
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if (chara == null) return;

            if (chara.IsSetTarget() == false) return;
            button.interactable = true;
        }

        public void OnClick()
        {

            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllManager.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.ExecuteAttack();

            //allParent.Off();
            //var chara = BCharacterManager.Instance.ActiveCharacter;
            //if (chara == null) return;
            //if (chara.singleAttack.IsSetTarget == true)
            //{
            //    chara.singleAttack.ExecuteAttack();
            //}
            //else if (chara.moveAttack.IsSetTarget == true)
            //{
            //    chara.moveAttack.ExcuteAttack();
            //}
        }
    }
}