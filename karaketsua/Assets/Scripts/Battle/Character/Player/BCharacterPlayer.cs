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
        [SerializeField]
        private BCharacterMoverManagerPlayer mover;
        //攻撃
        [SerializeField]
        private BCharacterAttackerManagerPlayer attacker;

        public event Action<BCharacterPlayer> OnActivePlayerE;
        public static event Action<BCharacterPlayer> OnActivePlayerStaticE;


        // Use this for initialization
        public override void Awake()
        {
            base.Awake();

        }

        public override void Start()
        {
            base.Start();

            //singleAttack = GetComponent<BCharacterSingleAttack>();
            //moveAttack = GetComponent<BCharacterMoveAttack>()

            SetWaitState();

        }

        public override void Initialize(IntVect2D array)
        {
            base.Initialize(array);
            isEnemy = false;


            OnActivePlayerE += BCharacterManager.Instance.SetActivePlayer;
        }

        public override bool IsNowAction()
        {
            return attacker.IsNowAction() || mover.IsNowAction();
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
        //isReactive=true:同一行動内での呼び出し
        public override void OnActive()
        {
            ResetActive();
            EnableMove();
            animator.SetDeffence(false);
        }
        public void ResetActive()
        {
            if (OnActivePlayerE != null) OnActivePlayerE(this);
            if (OnActivePlayerStaticE != null) OnActivePlayerStaticE(this);
            base.OnActive();
        }

        public override void OnEndActive()
        {
            mover.Reset();
            attacker.Reset();
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


        #region::UIからの行動
        //攻撃選択
        public void SelectAttack()
        {
            mover.IsEnable = false;
            attacker.IsEnable = false;
        }
        //通常攻撃選択
        public void SelectSingleAttack(int number)
        {
            attacker.IsEnable = true;
            attacker.SelectSingleAttack(number);
        }
        //移動攻撃選択
        public void SelectMoveAttack()
        {
            attacker.IsEnable = true;
            attacker.SelectMoveAttack();
        }
        public void ExecuteAttack()
        {
            attacker.ExecuteAttack();
        }

        //防御選択
        public void SelectDeffence()
        {
            mover.IsEnable=false;
        }

        //防御決定
        public void ExecuteDeffence()
        {
            animator.SetDeffence(true);
            OnEndActive();
        }
        public void BackToActionSelect()
        {
            ResetActive();
            mover.IsEnable = true;
        }

        #endregion::UIからの行動

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
