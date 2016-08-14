using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteAttackParent : UIBottomBase
    {

        public UIBottomExecuteAttack execute;

        public override void UpdateUI()
        {
            execute.UpdateUI();
        }

    }
}