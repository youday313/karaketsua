using UnityEngine;
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

    public class BCharacterBase : MonoBehaviour
    {
        //キャラクターパラメーター
        ///インスペクタから編集
        public CharacterParameter characterParameter;

        //キャタクターアクティブ時
        public event Action<BCharacterBase> OnActiveE;
        public static event Action<BCharacterBase> OnActiveStaticE;

        //キャタクター非アクティブ時
        public event Action<BCharacterBase> OnEndActiveE;
        public static event Action OnEndActiveStaticE;

        //キャラクター死亡時
        public event Action<BCharacterBase> OnDeathE;

        //ステータス変更
        public event Action<BCharacterBase> OnStatusUpdateE;

        public bool isEnemy = false;

        //座標
        //現在のキャラクター位置配列
        
        public IntVect2D positionArray = new IntVect2D(0, 0);

        //行動中
        public virtual bool IsNowAction()
        { return false; }

        //アニメーター
        protected BCharacterAnimator animator;

        //ライフ
        public BCharacterLife Life
        {
			get; private set;
        }

        //画面UIへの参照
        public BCharacterStateUI StateUI
        {
			get; private set;
        }

        //詳細UIへの参照
        private CharacterDetailStateUI detailUI=null;
        
        //アクティブサークル
        private GameObject activeCircle;
        
        //ターゲットサークル
        private GameObject targetCircle;

        public virtual void Init(IntVect2D array)
        {
            
            positionArray.x = array.x;
            positionArray.y = array.y;

            //ライフ設定
			Life.Init(characterParameter);
			OnEndActiveStaticE += BCharacterManager.Instance.ResetActiveCharacter;
        }


        public virtual void Awake()
        {
            Life = GetComponent<BCharacterLife>();
            animator = GetComponent<BCharacterAnimator>();
            //選択マーカー作成
            activeCircle= createCircle("ActiveCircle");
            targetCircle=createCircle("TargetCircle");
        }



        public virtual void Start()
        {
            //アクティブタイム作成
            createActiveTime();

            //位置変更
            setPositionOnTile();

            //UI作成
            createCharacterUI();

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
        public virtual void SetWaitState() {

        }

        #endregion

		private void createActiveTime()
        {
            var activeTime = BActiveTimeCreater.Instance.CreateActiveTime();
            activeTime.Init(this);
			if(isEnemy==true){
				BActiveTimeCreater.Instance.SetEnemyPosition (activeTime);

			}
            //SetActiveTimeEventHandler();
        }

        //タイルの上に移動
		private void setPositionOnTile()
        {
            var tilePosition = BBattleStage.Instance.GetTileXAndZPosition(positionArray);
            CSTransform.SetX(transform, tilePosition.x);
            CSTransform.SetZ(transform, tilePosition.y);
        }

		private void createCharacterUI()
        {
            StateUI = Instantiate(Resources.Load<BCharacterStateUI>("CharacterStateUI")) as BCharacterStateUI;
            StateUI.Init(this);
        }

        //キャラクターを行動選択状態にする
        public virtual void OnActive()
        {

            if(OnActiveE!=null)OnActiveE(this);
            if (OnActiveStaticE != null)
            {
                UIBottomCommandParent.UICommandState = EUICommandState.Action;
                OnActiveStaticE(this);
            }
   
            activeCircle.SetActive(true);
        }

        public virtual void OnEndActive()
        {
            if (OnEndActiveE != null) OnEndActiveE(this);
            if (OnEndActiveStaticE != null)
            {
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
            //リストから除く
            //WaitTimeManager.Instance.DestroyWaitTime(this.activeTime);
            //RemoveActiveTimeEventHandler();
            
            
            //activeTime.DeathCharacter();
            //CharacterManager.Instance.DestroyCharacter(this);
            if(OnDeathE!=null)OnDeathE(this);
            Destroy(gameObject);
        }

        public BCameraMove GetCamera()
        {
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BCameraMove>();
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

    }

}