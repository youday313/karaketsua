using UnityEngine;
using System.Collections;

using BattleScene;

namespace BattleScene
{

    public class UIBottomWaza : UIBottomBase
    {

        public UIBottomCommandParent commandParent;
        public int attackNumber;
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
            CharacterManager.Instance.GetActiveCharacter().singleAttack.selectActionNumber = attackNumber;
            CharacterManager.Instance.GetActiveCharacter().SelectSingleAttack();
            commandParent.CreateExecuteAttack();
        }
    }
}