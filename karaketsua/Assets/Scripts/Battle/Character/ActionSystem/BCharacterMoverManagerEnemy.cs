using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    [RequireComponent(typeof(BCharacterMoverAuto))]
    public class BCharacterMoverManagerEnemy : BCharacterActionManagerBase
    {
        BCharacterEnemy character;
        BCharacterMoverAuto mover;

        public void Awake()
        {
            character = GetComponent<BCharacterEnemy>();
            mover = GetComponent<BCharacterMoverAuto>();
            mover.OnCompleteMove += OnComplete;
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


            StartCoroutine(mover.StartAutoMove());
            
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
        public override bool IsNowAction()
        {
            return mover.IsNowAction;
        }
        public void OnComplete()
        {
            character.nowState = EnemyState.Moved;
        }

    }
}