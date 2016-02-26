using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterMoverManagerEnemy : BCharacterActionManagerBase
    {
        BCharacterMoverAuto mover;

        public void Awake()
        {
            mover = GetComponent<BCharacterMoverAuto>();
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
    }
}