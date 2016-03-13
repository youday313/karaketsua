using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    [RequireComponent(typeof(BCharacterMoverManual))]

    public class BCharacterMoverManagerPlayer : BCharacterActionManagerBase
    {
        BCharacterMoverManual mover;
        public void Awake()
        {
            mover = GetComponent<BCharacterMoverManual>();
        }
        public void Start()
        {
            Reset();
        }
        public override void Enable()
        {
            if (IsDone() == true)
                return;

            base.Enable();
            mover.IsEnable = true;

        }
        public override void Disable()
        {
            mover.IsEnable = false;
            base.Disable();
        }
        public override void Reset()
        {
            Disable();
            mover.Reset();
            base.Reset();
        }
        public override bool IsDone()
        {
            return mover.IsDone;
        }
        public override bool IsNowAction()
        {
            return mover.IsNowAction;
        }
    }
}