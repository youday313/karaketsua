using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterMoverManagerBase : BCharacterActionManagerBase
    {

        BCharacterMoverBase mover;

        public void Awake()
        {
            mover = GetComponent<BCharacterMoverBase>();
        }
        public void Start()
        {
            Reset();
        }

        public virtual void Enable()
        {
            if (IsDone() == true)
                return;

            mover.IsEnable = true;
        }

        public virtual void Disable()
        {
            base.Disable();
        }
        public virtual void Reset()
        {
            Disable();
            mover.Reset();
            base.Reset();
        }
        public virtual bool IsDone()
        {
            return mover.IsDone;
        }
}
}