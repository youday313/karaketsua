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
        BCharacterMoverManagerEnemy mover;

        //攻撃
        BCharacterAttackerManagerEnemy attacker;

        public event Action<BCharacterEnemy> OnActiveEnemyE;

        public override void Init(IntVect2D array)
        {

            base.Init(array);
            isEnemy = true;

            //回転
            transform.rotation = Quaternion.Euler(0, 180, 0);
            OnActiveEnemyE += BCharacterManager.Instance.SetActiveEnemy;
        }

        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            mover = GetComponent<BCharacterMoverManagerEnemy>();
            attacker = GetComponent<BCharacterAttackerManagerEnemy>();
        }

        public override void Start()
        {
            base.Start();
            //singleAttack = GetComponent<BCharacterSingleAttack>();
            //moveAttack = GetComponent<BCharacterMoveAttack>()
            SetWaitState();
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