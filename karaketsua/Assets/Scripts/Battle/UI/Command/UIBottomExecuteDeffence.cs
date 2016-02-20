using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{
    public class UIBottomExecuteDeffence : UIBottomBase
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

        public void OnClick()
        {
            //防御の実行
            CharacterManager.Instance.GetActiveCharacter().ExecuteDeffence();
            commandParent.CreateAction();
        }
    }
}