using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;
namespace BattleScene
{
    public class UIBottomAttack : UIBottomBase
    {
        Button button;
        public UIBottomCommandParent commandParent;
        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();

        }


        public override void UpdateUI()
        {
            button.interactable = false;
            //キャラクター状態取得
            var chara = CharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;
            if (chara.IsAttacked == true) return;

            button.interactable = true;

        }

        //ボタンクリック
        public void OnClick()
        {
            CharacterManager.Instance.GetActiveCharacter().SelectDisable();
            commandParent.CreateWazaSelect();
        }
    }
}