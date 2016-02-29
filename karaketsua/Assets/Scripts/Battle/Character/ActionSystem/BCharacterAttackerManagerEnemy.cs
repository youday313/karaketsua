using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    [RequireComponent(typeof(BCharacterAttackerAuto))]
    public class BCharacterAttackerManagerEnemy : BCharacterActionManagerBase
    {

        BCharacterAttackerAuto autoAttack;

        public void Awake()
        {
            autoAttack = GetComponent<BCharacterAttackerAuto>();
        }
        public void Start()
        {
            Reset();
        }

        public override void Enable()
        {
            autoAttack.IsEnable = true;
        }

        public override void Disable()
        {
            autoAttack.IsEnable = false;
            base.Disable();
        }
        public override void Reset()
        {
            Disable();
            autoAttack.Reset();
            base.Reset();
        }

        public override bool IsNowAction()
        {
            return autoAttack.IsNowAction;
        }
        public override bool IsDone()
        {
            return autoAttack.IsDone;
        }

    }
}