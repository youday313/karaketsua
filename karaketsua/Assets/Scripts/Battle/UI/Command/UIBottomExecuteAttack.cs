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
            var chara=CharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;
            if (chara.singleAttack.IsSetTarget == true)
            {
                button.interactable = true;
            }
            else if(chara.moveAttack.IsSetTarget==true){
                button.interactable = true;
            }
        }
        public void OnClick()
        {
            allParent.Off();
            var chara = CharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;
            if (chara.singleAttack.IsSetTarget == true)
            {
                chara.singleAttack.ExecuteAttack();
            }
            else if (chara.moveAttack.IsSetTarget == true)
            {
                chara.moveAttack.ExcuteAttack();
            }
              }
    }
}