using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteAttack : UIBottomBase
    {

        public UIBottomAllParent allParent;
        // Use this for initialization
        [SerializeField]Button button;

        void Awake()
        {
            button = GetComponent<Button>();
        }

        // Update is called once per frame
        public override void UpdateUI()
        {
            button.interactable = false;
            var chara=BCharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;

            if (chara.IsSetTarget() == false) return;
            button.interactable = true;
        }

        public void OnClick()
        {

            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllParent.Instance.UpdateUI();

            //allParent.Off();
            //var chara = BCharacterManager.Instance.GetActiveCharacter();
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