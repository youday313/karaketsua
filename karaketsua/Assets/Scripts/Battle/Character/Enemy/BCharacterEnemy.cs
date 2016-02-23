using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    public class BCharacterEnemy:MonoBehaviour
    {

        ///インスペクタから編集
        public CharacterParameter characterParameter;
        //現在のキャラクター位置配列
        [System.NonSerialized]
        public IntVect2D positionArray = new IntVect2D(0, 0);

        public BCharacterLife Life
        {
            get { return life; }
        }
        BCharacterLife life;
        BActiveTime activeTime;

        //アクティブか
        [System.NonSerialized]
        public bool isNowSelect = false;
        [System.NonSerialized]
        public bool IsAttacked = false;
        public GameObject activeCircle;

        public void Init(IntVect2D array)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
            positionArray.x = array.x;
            positionArray.y = array.y;
        }

        // Use this for initialization
        void Start()
        {
            //move = GetComponent<BCharacterMove>();
            //singleAttack = GetComponent<BCharacterSingleAttack>();
            life = GetComponent<BCharacterLife>();
            life.Init(characterParameter);
            //アクティブタイム作成
            //activeTime = BActiveTimeCreater.Instance.CreateActiveTime(this);
            SetActiveTimeEventHandler();

            //位置変更
            SetPositionOnTile();

            //選択マーカー表示
            activeCircle.SetActive(false);

            DisableActionMode();


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

        public void StartWave()
        {
            //move.IsEnable = false;
            //singleAttack.IsEnable = false;
        }


        //キャラクターを行動選択状態にする
        public void OnActive(BActiveTime aTime)
        {
            isNowSelect = true;
            EnableInitialActionMode();
            //CharacterManager.Instance.SetNowActiveCharacter(this);
            UIBottomAllParent.Instance.CreateAction();
            //ActionSelect.Instance.OnActiveCharacter(this);
            activeCircle.SetActive(true);
            //タイル変更
            //BattleStage.Instance.UpdateTileColors(this, TileState.Move);
        }

        //アクティブ状態
        void EnableInitialActionMode()
        {
            //GetCamera().SetActiveCharacter(this);
            SelectMove();
        }

        //移動可能
        public void SelectMove()
        {
            //move.IsEnable = true;
            //moveAttack.IsEnable = false;
            //singleAttack.IsEnable = false;
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
            //CharacterManager.Instance.DestroyCharacter(this);
            Destroy(gameObject);
        }



        BCameraMove GetCamera()
        {
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BCameraMove>();
        }

    }
}