using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

namespace BattleScene
{

    [RequireComponent(typeof(BCharacterMoverManagerEnemy))]
    [RequireComponent(typeof(BCharacterAttackerManagerEnemy))]
    public class BCharacterEnemy : BCharacterBase
    {
        public EnemyState nowState = EnemyState.Wait;
        //移動
        [SerializeField]
        private BCharacterMoverManagerEnemy mover;

        //攻撃
        [SerializeField]
        private BCharacterAttackerManagerEnemy attacker;

        public event Action<BCharacterEnemy> OnActiveEnemyE;

        public override void Initialize(IntVect2D array)
        {
            isEnemy = true;
            base.Initialize(array);

            //回転
            transform.rotation = Quaternion.Euler(0, 180, 0);
            OnActiveEnemyE += BCharacterManager.Instance.SetActiveEnemy;
        }


        public override bool IsNowAction()
        {
            return attacker.IsNowAction() == true && mover.IsNowAction() == true;
        }

        public override bool IsAttacked()
        {
            return attacker.IsDone();
        }

        //非選択状態
        public override void SetWaitState()
        {
            mover.IsEnable = false;
            attacker.IsEnable = false;
        }





        //キャラクターを行動選択状態にする
        public override void OnActive()
        {
            base.OnActive();
            if (OnActiveEnemyE != null) OnActiveEnemyE(this);
            nowState = EnemyState.Active;
        }

        public override void OnEndActive()
        {
            mover.Reset();
            attacker.Reset();
            nowState = EnemyState.Wait;
            base.OnEndActive();

            SetWaitState();
        }

        // 毎フレーム処理
        // パターンで切り替え
        void Update()
        {
            switch (nowState)
            {
                case EnemyState.Active:
                    {
                        nowState = EnemyState.MoveStart;
                        mover.IsEnable = true;
                        break;
                    }
                case EnemyState.Moved:
                    {
                        nowState = EnemyState.AttackStart;
                        attacker.IsEnable = true;
                        break;
                    }
                case EnemyState.Attacked:
                    {
                        OnEndActive();
                        break;
                    }
                default: break;
            }
        }

    }
}