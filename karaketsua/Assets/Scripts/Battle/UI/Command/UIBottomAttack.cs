using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;
namespace BattleScene
{
    public class UIBottomAttack : UIBottomBase
    {
        Button button;
        //public UIBottomCommandParent commandParent;
        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();
            

        }
        void Start()
        {
            
        }

        public override void UpdateUI()
        {
            button.interactable = false;
            var chara = BCharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;
            if (chara.IsAttacked || chara.isEnemy||chara.IsNowAction) return;         
            button.interactable = true;

        }

        //ボタンクリック
        public void OnClick()
        {
            //BCharacterManager.Instance.GetActiveCharacter().SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.Waza;
            UIBottomAllParent.Instance.UpdateUI();
        }
    }
}