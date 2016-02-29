using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

namespace BattleScene
{


    [RequireComponent(typeof(BCharacterMoverManagerPlayer))]
    [RequireComponent(typeof(BCharacterAttackerManagerPlayer))]

    public class BCharacterPlayer : BCharacterBase
    {

        //移動
        BCharacterMoverManagerPlayer mover;

        //攻撃
        BCharacterAttackerManagerPlayer attacker;

        public static event Action OnActivePlayerStaticE;


        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            mover = GetComponent<BCharacterMoverManagerPlayer>();
            attacker = GetComponent<BCharacterAttackerManagerPlayer>();

        }

        public override void Start()
        {
            base.Start();
            //singleAttack = GetComponent<BCharacterSingleAttack>();
            //moveAttack = GetComponent<BCharacterMoveAttack>()

            SetWaitState();

        }

        public override void Init(IntVect2D array)
        {
            base.Init(array);
            isEnemy = false;
        }

        public override bool IsNowAction()
        {
            return attacker.IsNowAction() == true && mover.IsNowAction() == true;
        }

        public override bool IsAttacked()
        {
            return attacker.IsDone();
        }

        public override bool IsSetTarget()
        {
            return attacker.IsSetTarget();
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
            if (OnActivePlayerStaticE != null) OnActivePlayerStaticE();
            EnableMove();
        }

        public void OnEndActive()
        {
            base.OnEndActive();
            //BCharacterManager.Instance.SetNowActiveCharacter(null);
            //BBattleStage.Instance.ResetAllTileColor();
            //activeTime.ResetValue();
            //SetWaitState();
            //GetCamera().DisableActiveCharacter();
            //UIBottomAllParent.Instance.CreateAction();
            //ActionSelect.Instance.EndActiveAction();

            SetWaitState();
        }

        //移動可能
        public void EnableMove()
        {
            mover.IsEnable = true;
        }




        //ボタンからの行動
        public void ExecuteDeffence()
        {
            OnEndActive();
        }


        ////移動可能
        //public void SelectMove()
        //{
        //    mover.IsEnable = true;
        //    moveAttack.IsEnable = false;
        //    singleAttack.IsEnable = false;
        //}
        ////攻撃選択
        //public void SelectSingleAttack()
        //{
        //    mover.Reset();
        //    singleAttack.IsEnable = true;
        //    moveAttack.IsEnable = false;
        //}
        ////移動攻撃選択
        //public void SelectMoveAttack()
        //{
        //    mover.Reset();
        //    singleAttack.IsEnable = false;
        //    moveAttack.IsEnable = true;
        //}
        ////行動不可能
        //public void SelectDisable()
        //{
        //    mover.Reset();
        //    singleAttack.IsEnable = false;
        //    moveAttack.IsEnable = false;
        //}
        //public void StartWave()
        //{
        //    mover.IsEnable = false;
        //    singleAttack.IsEnable = false;
        //    moveAttack.IsEnable = false;
        //}


        ////死の実行
        //public void DeathMyself()
        //{
        //    //爆発エフェクト
        //    //Instantiate(Resources.Load<GameObject>("DeathEffect"), transform.position, Quaternion.identity);
        //    //リストから除く
        //    //WaitTimeManager.Instance.DestroyWaitTime(this.activeTime);
        //    RemoveActiveTimeEventHandler();
        //    activeTime.DeathCharacter();
        //    BCharacterManager.Instance.RemovePlayer(this);
        //    Destroy(gameObject);
        //}

    }

}
