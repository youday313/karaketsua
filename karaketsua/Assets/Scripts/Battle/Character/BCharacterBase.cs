﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using BattleScene;

namespace BattleScene
{
    //敵、味方キャラクターの基本クラス
    [RequireComponent(typeof(BCharacterAnimator))]
    [RequireComponent(typeof(BCharacterLife))]

    public class BCharacterBase: MonoBehaviour
    {
        // キャラクターパラメーター
        // インスペクタから編集
        public CharacterMasterParameter characterParameter;

        // キャタクターアクティブ時
        public event Action<BCharacterBase> OnActiveE;
        public static event Action<BCharacterBase> OnActiveStaticE;

        // キャタクター非アクティブ時
        public event Action<BCharacterBase> OnEndActiveE;
        public static event Action OnEndActiveStaticE;

        // キャラクター死亡時
        public event Action<BCharacterBase> OnDeathE;

        // ステータス変更
        public event Action OnStatusUpdateE;

        public bool IsEnemy = false;

        // 座標
        // 現在のキャラクター位置配列
        public IntVect2D PositionArray = new IntVect2D(0, 0);

        //行動中
        public virtual bool IsNowAction()
        { return false; }

        //アニメーター
        [SerializeField]
        protected BCharacterAnimator animator;

        //ライフ
        [SerializeField]
        private BCharacterLife life;
        public BCharacterLife Life { get { return life; } }

        //詳細UIへの参照
        private CharacterDetailStateUI detailUI = null;

        //アクティブサークル
        private GameObject activeCircle;
        //ターゲットサークル
        private GameObject targetCircle;

        // 共通初期化
        public virtual void Initialize(CharacterMasterParameter param, IntVect2D array)
        {
            characterParameter = param;
            //選択マーカー作成
            activeCircle = createCircle("ActiveCircle");
            targetCircle = createCircle("TargetCircle");

            PositionArray = IntVect2D.Clone(array);
            //ライフ設定
            Life.Initialize(characterParameter);
            OnEndActiveStaticE += BCharacterManager.Instance.ResetActiveCharacter;

            //アクティブタイム作成
            BActiveTimeCreater.Instance.Initialize(this);

            //位置変更
            setPositionOnTile();
            //回転
            ResetRotate();
            //UI作成
            BStateUICreater.Instance.Create(this);
        }


        void Start()
        {
            SetWaitState();
        }


        #region::継承の仮想関数

        public virtual bool IsAttacked()
        {
            return false;
        }
        public virtual bool IsSetTarget()
        {
            return false;
        }
        public virtual void SetWaitState()
        {

        }

        #endregion


        //タイルの上に移動
        private void setPositionOnTile()
        {
            var tilePosition = BBattleStage.Instance.GetTileXAndZPosition(PositionArray);
            CSTransform.SetX(transform, tilePosition.x);
            CSTransform.SetZ(transform, tilePosition.y);
        }


        //キャラクターを行動選択状態にする
        public virtual void OnActive()
        {
            if(OnActiveE != null) {
                OnActiveE(this);
            }
            if(OnActiveStaticE != null) {
                UIBottomCommandParent.UICommandState = EUICommandState.Action;
                OnActiveStaticE(this);
            }

            activeCircle.SetActive(true);
        }

        // 行動終了の共通処理
        public virtual void OnEndActive()
        {
            if(OnEndActiveE != null) {
                OnEndActiveE(this);
            }
            if(OnEndActiveStaticE != null) {
                UIBottomCommandParent.UICommandState = EUICommandState.Action;
                OnEndActiveStaticE();
            }
            activeCircle.SetActive(false);
        }


        //死の実行
        public void DeathMyself()
        {
            //爆発エフェクト
            //Instantiate(Resources.Load<GameObject>("DeathEffect"), transform.position, Quaternion.identity);

            if(OnDeathE != null) {
                OnDeathE(this);
                BCharacterManager.Instance.Remove(this);
            }
            Destroy(gameObject);
        }

        private GameObject createCircle(string objName)
        {
            var obj = Instantiate(Resources.Load<GameObject>(objName));
            obj.transform.SetParent(this.gameObject.transform);
            obj.SetActive(false);
            return obj;
        }
        public void SetTargeted(bool isTarget)
        {
            targetCircle.SetActive(isTarget);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        // イベント発火用
        public void StatusUpdate()
        {
            if(OnStatusUpdateE != null) {
                OnStatusUpdateE();
            }
        }

        // 向きをデフォルトに戻す
        public void ResetRotate()
        {
            var isEnemyAngle = IsEnemy == true ? 180 : 0;
            transform.eulerAngles = new Vector3(0, isEnemyAngle, 0);
        }
    }
}