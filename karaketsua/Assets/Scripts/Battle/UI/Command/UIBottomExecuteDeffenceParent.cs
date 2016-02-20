using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteDeffenceParent : UIBottomBase
    {

        public UIBottomExecuteDeffence execute;
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
            execute.UpdateUI();
        }
    }
}