using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;
namespace BattleScene
{
    public class UIBottomDeffence : UIBottomBase
    {
        Button button;
        public UIBottomCommandParent commandParent;
        // Use this for initialization
        void Awake()
        {
            button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateUI()
        {
            button.interactable = false;
            //キャラクター状態取得
            var chara = CharacterManager.Instance.GetActiveCharacter();
            if (chara == null) return;

            button.interactable = true;
        }

        public void OnClick()
        {
            CharacterManager.Instance.GetActiveCharacter().SelectDisable();
            commandParent.CreateExecuteDeffence();
        }
    }
}
