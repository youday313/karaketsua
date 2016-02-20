using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{

    public class UIBottomSpecialWaza : UIBottomBase
    {

        public UIBottomCommandParent commandParent;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void UpdateUI()
        {
            base.UpdateUI();
        }
        public void OnClick()
        {
            CharacterManager.Instance.GetActiveCharacter().SelectMoveAttack();
            commandParent.CreateExecuteAttack();
        }
    }
}