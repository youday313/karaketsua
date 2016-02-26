using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using BattleScene;

namespace BattleScene
{
    public class UIBottomActionParent : UIBottomBase
    {
        public UIBottomAttack attack;
        public UIBottomDeffence deffence;
        // Use this for initialization

        public override void UpdateUI()
        {
            attack.UpdateUI();
            deffence.UpdateUI();
        }
    }
}