using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    [RequireComponent(typeof(BCharacterAttackerSingle))]
    [RequireComponent(typeof(BCharacterAttackerMove))]

    public class BCharacterAttackerManagerPlayer : BCharacterActionManagerBase
    {

        BCharacterAttackerSingle singleAttack;
        BCharacterAttackerMove moveAttack;


        public void Awake()
        {
            singleAttack = GetComponent<BCharacterAttackerSingle>();
            moveAttack = GetComponent<BCharacterAttackerMove>();
        }
        public void Start()
        {
            Reset();
        }

        public void SelectSingleAttack()
        {
            singleAttack.IsEnable = true;
        }
        public void SelectMoveAttack()
        {
            moveAttack.IsEnable = true;
        }

        public override void Disable()
        {
            singleAttack.IsEnable = false;
            moveAttack.IsEnable = false;
        }
        public override void Reset()
        {
            Disable();
            singleAttack.Reset();
            moveAttack.Reset();
            base.Reset();
        }

        public override bool IsNowAction()
        {
            return singleAttack.IsNowAction || moveAttack.IsNowAction;
        }

        public override bool IsDone()
        {
            return singleAttack.IsDone || moveAttack.IsDone;
        }
    }

}