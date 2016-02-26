using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterAttackerMove : BCharacterAttackerBase
    {

        //選択した攻撃方法
        [System.NonSerialized]
        public MoveAttackParameter selectAttackParameter = null;

        //現在なぞっている範囲
        List<IntVect2D> nowTraceTiles = new List<IntVect2D>();
        [System.NonSerialized]
        public bool isNowCharge = false;
        GameObject startAttackPanel;
        //移動攻撃可能になった
        bool isNowAttackable = false;
        //攻撃モーション時間
        //モーション時間＋猶予時間の案もありか
        public float attackMotionTime = 3f;

        //攻撃可能
        public bool IsAttackable
        {
            get
            {
                if (selectAttackParameter == null||IsSetTarget==false) return false;
                else
                {
                    return nowTraceTiles.Count == selectAttackParameter.moveRange;
                }
            }
        }


        public override void Enable()
        {
            base.Enable();
            IT_Gesture.onDraggingStartE += OnDraggingStart;
        }
        public override void Disable()
        {
            IT_Gesture.onDraggingStartE -= OnDraggingStart;
            nowTraceTiles = new List<IntVect2D>();
            isNowAttackable = false;

            //ActionSelect.Instance.DisableMoveAttackButton();
            base.Disable();
        }
        //なぞり開始
        void OnDraggingStart(DragInfo dragInfo)
        {
            isNowAttackable = false;
            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if (tilePosition == null) return;

            //キャラクターの上
            if (IntVect2D.IsEqual(tilePosition, character.positionArray) == false) return;
            BBattleStage.Instance.ResetAllTileColor();

            nowTraceTiles = new List<IntVect2D>();
            nowTraceTiles.Add(tilePosition);

            //タイル
            BBattleStage.Instance.OnSelecSkillTarget(tilePosition);

            IT_Gesture.onDraggingE += OnDragging;
            IT_Gesture.onDraggingEndE += OnDraggingEnd;
            isNowCharge = true;
            //ActionSelect.Instance.DisableMoveAttackButton();
        }
        void OnDragging(DragInfo dragInfo)
        {
            //もうなぞれない
            if (nowTraceTiles.Count == selectAttackParameter.moveRange) return;

            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if (tilePosition == null) return;
            //まだ未通過
            if (IntVect2D.IsEqual(tilePosition, nowTraceTiles.LastOrDefault())) return;
            //まだ未完成
            if (selectAttackParameter.moveRange == nowTraceTiles.Count) return;

            //次のタイル
            if (IntVect2D.IsNeighbor(tilePosition, nowTraceTiles.LastOrDefault()) == false) return;

            nowTraceTiles.Add(tilePosition);
            BBattleStage.Instance.ChangeColor(tilePosition, TileState.Skill);

        }
        void OnDraggingEnd(DragInfo dragInfo)
        {

            CheckTraceComplete();
            //nowTraceTiles = new List<IntVect2D>();
            IT_Gesture.onDraggingE -= OnDragging;
            isNowCharge = false;
            IT_Gesture.onDraggingEndE -= OnDraggingEnd;
        }

        void CheckTraceComplete()
        {
            //BattleStage.Instance.ResetAllTileColor();
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();

            //距離をなぞった
            //失敗
            if (nowTraceTiles.Count != selectAttackParameter.moveRange)
            {
                foreach (var tile in nowTraceTiles)
                {
                    BBattleStage.Instance.ResetAllTileColor();
                }
                return;
            }
            //最後のタイルにキャラがいない
            //キャラクターが既にいない
            if (BCharacterManager.Instance.IsExistCharacterOnTile(nowTraceTiles.Last()) == true) return;

            //成功

            SetTarget();
            //攻撃範囲に敵がいない
            if (attackTarget.Count == 0)
            {
                return;
            }

            //攻撃
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();
            //Attack();
            //Disable();
        }
        void SetTarget()
        {
            foreach (var traceTile in nowTraceTiles)
            {
                var target = BCharacterManager.Instance.GetOpponentCharacterOnTileFormVect2D(traceTile,character.isEnemy);
                if (target == null) continue;
                attackTarget.Add(target);
            }
        }

        //攻撃実行
        public void ExecuteAttack()
        {

            if (attackTarget.Count == 0) return;

            //攻撃
            foreach (var target in attackTarget)
            {
                target.Life.Damage(character.characterParameter.power);
                target.Life.CheckDestroy();
            }

            attackTarget = null;


            StartAttackAnimation();
            isDone = true;
            //攻撃時にUI非表示
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllParent.Instance.Off();

            Disable();
        }
        void StartAttackAnimation()
        {
            animator.SetMoveAttack();
            isNowAction = true;

            //コルーチンで移動
            StartCoroutine("Move");
            attackMotionTime = 7f;
            Invoke("OnCompleteAnimation", attackMotionTime);
        }

        //移動コルーチン
        float tileMoveTime = 0.03f;
        IEnumerator Move()
        {
            foreach (var trace in nowTraceTiles)
            {

                UpdatePosition(trace);

                yield return new WaitForSeconds(tileMoveTime);
            }


        }
        //実際に移動
        void UpdatePosition(IntVect2D newPosition)
        {

            //移動先のタイル位置
            var toTilePostion = BBattleStage.Instance.GetTileXAndZPosition(newPosition);
            var table = GetMoveTable(toTilePostion);

            //座標変更
            iTween.MoveTo(gameObject, table);


            //配列値変更
            character.positionArray = newPosition;
        }

        Hashtable GetMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x", position.x);
            table.Add("z", position.y);
            //table.Add("time", 1.0f);
            table.Add("time", tileMoveTime);
            table.Add("easetype", iTween.EaseType.linear);
            return table;
        }


        void OnCompleteAnimation()
        {

            isNowAction = false;
            BCameraChange.Instance.ActiveLeanMode();
            //行動終了
            character.OnEndActive();
        }




    }
}