using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteDeffenceParent : UIBottomBase
    {

        public UIBottomExecuteDeffence execute;


        public override void UpdateUI()
        {
            execute.UpdateUI();
        }
    }
}