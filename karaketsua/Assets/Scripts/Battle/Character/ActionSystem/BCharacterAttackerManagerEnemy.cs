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
        BCharacterEnemy character;
        BCharacterAttackerAuto autoAttack;

        public void Awake()
        {
            character = GetComponent<BCharacterEnemy>();
            autoAttack = GetComponent<BCharacterAttackerAuto>();
            autoAttack.OnComplete += OnComplete;
        }
        public void Start()
        {
            Reset();
        }

        public override void Enable()
        {
            autoAttack.IsEnable = true;
            autoAttack.StartAutoAttack();
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
        public void OnComplete()
        {
            character.nowState = EnemyState.Attacked;
        }
    }
}