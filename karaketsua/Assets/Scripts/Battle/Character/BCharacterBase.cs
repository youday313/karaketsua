using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
//敵、味方キャラクターの基本クラス

    public class BCharacterBase : MonoBehaviour
    {
        //キャラクターパラメーター
        ///インスペクタから編集
        public CharacterParameter characterParameter;
        //座標
        //現在のキャラクター位置配列
        [System.NonSerialized]
        public IntVect2D positionArray = new IntVect2D(0, 0);

        //攻撃能力

        //移動能力

        //ライフ
        public BCharacterLife Life
        {
            get { return life; }
        }
        BCharacterLife life;
        
        //アクティブゲージ
        BActiveTime activeTime;


        public void Init(IntVect2D array)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            positionArray.x = array.x;
            positionArray.y = array.y;
        }

        //Startメソッドのかわり
        public void OnStart()
        {
            //move = GetComponent<BCharacterMove>();
            //singleAttack = GetComponent<BCharacterSingleAttack>();
            life = GetComponent<BCharacterLife>();
            life.Init(characterParameter);
            //アクティブタイム作成
            //activeTime = BActiveTimeCreater.Instance.CreateActiveTime(this);
            //SetActiveTimeEventHandler();

            //位置変更
            //SetPositionOnTile();

            //選択マーカー表示
            //activeCircle.SetActive(false);

            //DisableActionMode();
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


        //キャラクターを行動選択状態にする
        public void OnActive(BActiveTime aTime)
        {
            EnableInitialActionMode();
            //CharacterManager.Instance.SetNowActiveCharacter(this);
            //UIBottomAllParent.Instance.CreateAction();
            //ActionSelect.Instance.OnActiveCharacter(this);
            //activeCircle.SetActive(true);
            //タイル変更
            //BattleStage.Instance.UpdateTileColors(this, TileState.Move);
        }

        //アクティブ状態
        public virtual void EnableInitialActionMode()
        {
            //GetCamera().SetActiveCharacter(this);
            //SelectMove();
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


        public BCameraMove GetCamera()
        {
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BCameraMove>();
        }

    }

}