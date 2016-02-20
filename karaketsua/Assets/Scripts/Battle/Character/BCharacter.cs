using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    #region::パラメーター
    //状態異常
    public enum StateError { 炎焼,凍結,裂傷,感電,石化,毒,封印,混乱}
    [System.Serializable]
    public class CharacterParameter
    {
        //キャラ名
        public string charaName;
        //体力
        public int HP;
        //攻撃技関連
        public List<AttackParameter> attackParameter=new List<AttackParameter>();
        //行動力
        public int activeSpeed;
        //攻撃力
        public int power;
        //属性攻撃力
        public List<int> elementPower=new List<int>();
        //防御力
        public int deffence;
        //属性防御力
        public List<int> elementDeffence=new List<int>(); 
        //精神力,MP
        public int skillPoint;
        //知力
        public int intellect;
        //気力,移動攻撃用
        public int movePoint;
        //状態異常
        public List<StateError> stateError=new List<StateError>();

    }

    #endregion::パラメーター

    [RequireComponent(typeof(BCharacterMove))]

    public class BCharacter : MonoBehaviour
    {

        BCharacterMove move;
        [System.NonSerialized]
        public BCharacterSingleAttack singleAttack;
        [System.NonSerialized]
        public BCharacterMoveAttack moveAttack;
        [System.NonSerialized]
        public bool IsAttacked=false;
        public BCharacterLife Life{
            get{return life;}
        }
        BCharacterLife life;
        BActiveTime activeTime;
        public BCharacterStateUI StateUI
        {
            get { return stateUI; }
        }
        BCharacterStateUI stateUI;

        //アクティブか
        [System.NonSerialized]
        public bool isNowSelect = false;
        //現在のキャラクター位置配列
        [System.NonSerialized]
        public IntVect2D positionArray = new IntVect2D(0, 0);
        [System.NonSerialized]
        public bool isEnemy = false;

        public GameObject activeCircle;
        
        ///インスペクタから編集
        public CharacterParameter characterParameter;


        public void Init(IntVect2D array, bool isEne)
        {
            isEnemy = isEne;
            if (isEnemy == true)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

            }
            positionArray.x = array.x;
            positionArray.y = array.y;
        }

        // Use this for initialization
        void Start()
        {
            move = GetComponent<BCharacterMove>();
            singleAttack = GetComponent<BCharacterSingleAttack>();
            moveAttack = GetComponent<BCharacterMoveAttack>();
            life = GetComponent<BCharacterLife>();
            life.Init(characterParameter);
            //アクティブタイム作成
            activeTime = BActiveTimeCreater.Instance.CreateActiveTime(this);
            SetActiveTimeEventHandler();

            //位置変更
            SetPositionOnTile();

            //選択マーカー表示
            activeCircle.SetActive(false);

            DisableActionMode();

            //UI作成
            CreateCharacterUI();

        }
        //アクティブタイムに登録
        void SetActiveTimeEventHandler()
        {
            activeTime.OnStopActiveTimeE += OnActive;
        }
        //アクティブタイムから削除
        void RemoveActiveTimeEventHandler()
        {
            activeTime.OnStopActiveTimeE -= OnActive;
        }
        //タイルの上に移動
        void SetPositionOnTile()
        {
            var tilePosition = BBattleStage.Instance.GetTile(positionArray).transform.position;
            CSTransform.SetX(transform, tilePosition.x);
            CSTransform.SetZ(transform, tilePosition.z);
        }
        //非選択状態
        void DisableActionMode()
        {
            isNowSelect = false;
            StartWave();
            IsAttacked = false;
            activeCircle.SetActive(false);
        }
        void CreateCharacterUI()
        {
            if (isEnemy == true) return;
            stateUI = Instantiate(Resources.Load<BCharacterStateUI>("CharacterStateUI")) as BCharacterStateUI;
            stateUI.Init(this);
        }

        public bool IsNowAction()
        {
            if (move.isNowAction == true) return true;
            if (singleAttack.isNowAction == true) return true;
            if (moveAttack.isNowAction == true) return true;
            return false;
        }


        //キャラクターを行動選択状態にする
        public void OnActive(BActiveTime aTime)
        {
            isNowSelect = true;
            EnableInitialActionMode();
            CharacterManager.Instance.SetNowActiveCharacter(this);
            UIBottomAllParent.Instance.CreateAction();
            //ActionSelect.Instance.OnActiveCharacter(this);
            activeCircle.SetActive(true);
            //タイル変更
            //BattleStage.Instance.UpdateTileColors(this, TileState.Move);
        }
        //アクティブ状態
        void EnableInitialActionMode()
        {
            GetCamera().SetActiveCharacter(this);
            SelectMove();
        }

        //ボタンからの行動
        public void ExecuteDeffence()
        {
            EndActiveCharacterAction();
        }
        //移動可能
        public void SelectMove()
        {
            move.IsEnable = true;
            moveAttack.IsEnable = false;
            singleAttack.IsEnable = false;
        }
        //攻撃選択
        public void SelectSingleAttack()
        {
            move.Reset();
            singleAttack.IsEnable = true;
            moveAttack.IsEnable = false;
        }
        //移動攻撃選択
        public void SelectMoveAttack()
        {
            move.Reset();
            singleAttack.IsEnable = false;
            moveAttack.IsEnable = true;
        }
        //行動不可能
        public void SelectDisable()
        {
            move.Reset();
            singleAttack.IsEnable = false;
            moveAttack.IsEnable = false;
        }
        public void StartWave()
        {
            move.IsEnable = false;
            singleAttack.IsEnable = false;
            moveAttack.IsEnable = false;
        }
        
        

        //行動終了
        public void EndActiveCharacterAction()
        {
            CharacterManager.Instance.SetNowActiveCharacter(null);
            BBattleStage.Instance.ResetAllTileColor();
            activeTime.ResetValue();
            DisableActionMode();
            //PlayerOwner.Instance.OnEndActive();
            GetCamera().DisableActiveCharacter();
            UIBottomAllParent.Instance.CreateAction();
            //ActionSelect.Instance.EndActiveAction();
        }

        //死の実行
        public void DeathMyself()
        {
            //爆発エフェクト
            //Instantiate(Resources.Load<GameObject>("DeathEffect"), transform.position, Quaternion.identity);
            //リストから除く
            //WaitTimeManager.Instance.DestroyWaitTime(this.activeTime);
            RemoveActiveTimeEventHandler();
            activeTime.DeathCharacter();
            CharacterManager.Instance.DestroyCharacter(this);
            Destroy(gameObject);
        }




        BCameraMove GetCamera()
        {
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BCameraMove>();
        }



    }

}
