using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

namespace BattleScene
{

    public class BCharacterEnemy:BCharacterBase
    {

        //移動
        BCharacterMoverManagerEnemy mover;

        //攻撃
        BCharacterAttackerManagerEnemy attacker;

        public static event Action OnActiveEnemyStaticE;

        public override void Init(IntVect2D array)
        {
            
            base.Init(array);
            isEnemy = true;
            //回転
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
        public void OnActive()
        {
            base.OnActive();
            if (OnActiveEnemyStaticE != null) OnActiveEnemyStaticE();

        }

        public void OnEndActive()
        {
            base.OnEndActive();

            SetWaitState();
        }

    }
}