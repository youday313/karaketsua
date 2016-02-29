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
            if (chara.isEnemy || chara.IsNowAction()) return;  
            button.interactable = true;
        }


        public void OnClick()
        {
            //BCharacterManager.Instance.GetActiveCharacter().SelectDisable();
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteDeffence;
            UIBottomAllParent.Instance.UpdateUI();
        }
    }
}
