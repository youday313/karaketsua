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
            var chara = BCharacterManager.Instance.ActiveCharacter;
            if (chara == null) return;
            if (chara.IsAttacked()==true || chara.isEnemy==true||chara.IsNowAction()==true) return;
            button.interactable = true;
        }

        //ボタンクリック
        public void OnClick()
        {
            //BCharacterManager.Instance.ActiveCharacter.SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.Waza;
            UIBottomAllParent.Instance.UpdateUI();
            var chara = BCharacterManager.Instance.ActivePlayer;
            if (chara == null) return;
            chara.SelectAttack();
        }
    }
}