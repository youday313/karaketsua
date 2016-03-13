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
        public override void Enable()
        {
            base.Enable();
        }

        public void SelectSingleAttack(int selectNumber)
        {
            singleAttack.IsEnable = true;
            singleAttack.SelectWazaNumber = selectNumber;

        }
        public void SelectMoveAttack()
        {
            moveAttack.IsEnable = true;
        }
        public void ExecuteAttack()
        {
            if (singleAttack.IsEnable == true)
            {
                singleAttack.ExecuteAttack();
            }
            else if (moveAttack.IsEnable == true)
            {
                moveAttack.ExecuteAttack();
            }
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

        public bool IsSetTarget()
        {
            return singleAttack.IsSetTarget == true || moveAttack.IsSetTarget == true;
        }
    }

}