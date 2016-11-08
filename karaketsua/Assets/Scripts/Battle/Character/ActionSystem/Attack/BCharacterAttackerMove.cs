using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterAttackerMove: BCharacterAttackerBase
    {

        //選択した攻撃方法
        [System.NonSerialized]
        public MoveAttackParameter selectAttackParameter = null;

        //現在なぞっている範囲
        private List<IntVect2D> nowTraceTiles = new List<IntVect2D>();
        //攻撃モーション時間
        //モーション時間＋猶予時間の案もありか
        [SerializeField]
        private float attackMotionTime = 3f;
        [SerializeField]
        private float tileMoveTime = 1f;

        //攻撃可能
        public bool IsAttackable {
            get {
                if(selectAttackParameter == null || IsSetTarget == false)
                    return false;
                else {
                    return nowTraceTiles.Count == selectAttackParameter.moveRange;
                }
            }
        }
            
        public override void Enable()
        {
            base.Enable();
            selectAttackParameter = character.characterParameter.moveAttackParameter;
            IT_Gesture.onDraggingStartE += onDraggingStart;
            BCameraManager.Instance.ActiveUpMode();
        }

        public override void Disable()
        {
            IT_Gesture.onDraggingStartE -= onDraggingStart;
            nowTraceTiles = new List<IntVect2D>();
            base.Disable();
        }
        //なぞり開始
        private void onDraggingStart(DragInfo dragInfo)
        {
            if(IsDone == true)
                return;
            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if(tilePosition == null)
                return;

            //キャラクターの上
            if(IntVect2D.IsEqual(tilePosition, character.PositionArray) == false)
                return;
            BBattleStage.Instance.ResetAllTileColor();

            nowTraceTiles = new List<IntVect2D>();
            nowTraceTiles.Add(tilePosition);
            foreach(var target in TargetList) {
                target.SetTargeted(false);
            }
            TargetList = new List<BCharacterBase>();

            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();
            //タイル
            BBattleStage.Instance.OnSelecSkillTarget(tilePosition);

            IT_Gesture.onDraggingE += onDragging;
            IT_Gesture.onDraggingEndE += onDraggingEnd;
            //ActionSelect.Instance.DisableMoveAttackButton();
        }

        private void onDragging(DragInfo dragInfo)
        {
            //もうなぞれない
            if(nowTraceTiles.Count == selectAttackParameter.moveRange)
                return;

            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if(tilePosition == null)
                return;
            //まだ未通過
            if(IntVect2D.IsEqual(tilePosition, nowTraceTiles.LastOrDefault()))
                return;
            //まだ未完成
            if(selectAttackParameter.moveRange == nowTraceTiles.Count)
                return;

            //次のタイル
            if(IntVect2D.IsNeighbor(tilePosition, nowTraceTiles.LastOrDefault()) == false)
                return;

            nowTraceTiles.Add(tilePosition);
            BBattleStage.Instance.ChangeColor(tilePosition, TileState.Skill);

        }

        private void onDraggingEnd(DragInfo dragInfo)
        {

            checkTraceComplete();
            IT_Gesture.onDraggingE -= onDragging;
            IT_Gesture.onDraggingEndE -= onDraggingEnd;
        }

        private void checkTraceComplete()
        {
            //BattleStage.Instance.ResetAllTileColor();
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();

            //距離をなぞった
            //失敗
            if(nowTraceTiles.Count != selectAttackParameter.moveRange) {
                foreach(var tile in nowTraceTiles) {
                    BBattleStage.Instance.ResetAllTileColor();
                }
                return;
            }
            //最後のタイルにキャラがいない
            //キャラクターが既にいない
            if(BCharacterManager.Instance.IsExistCharacterOnTile(nowTraceTiles.Last()) == true)
                return;

            //成功

            setTarget();
            //攻撃範囲に敵がいない
            if(TargetList.Count == 0) {
                return;
            }
            // 攻撃範囲に味方がいない
            if(TargetList.Any(x => !x.IsEnemy)) {
                return;
            }

            //攻撃
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();
        }

        private void setTarget()
        {
            foreach(var traceTile in nowTraceTiles) {
                var target = BCharacterManager.Instance.GetOpponentCharacterOnTileFormVect2D(traceTile, character.IsEnemy);
                if(target == null)
                    continue;

                TargetList.Add(target);
                target.SetTargeted(true);
            }
        }

        //攻撃実行
        public void ExecuteAttack()
        {
            if(TargetList.Count == 0) {
                return;
            }
            startMoveForAttack();
            IsDone = true;
            //攻撃時にUI非表示
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllManager.Instance.Off();
        }

        private void startMoveForAttack()
        {
            // 後ろから映す
            BCameraManager.Instance.StartMoveAttack(transform);

            // キャラクターアニメーションスタート
            animator.SetMoveAttack();
            IsNowAction = true;
            HideOtherCharacters();

            //　コルーチンで移動開始
            StartCoroutine(move());
        }

        //移動コルーチン
        private IEnumerator move()
        {
            foreach(var trace in nowTraceTiles) {
                updatePosition(trace);
                yield return new WaitForSeconds(tileMoveTime);
            }
            yield return new WaitForSeconds(attackMotionTime);
            onCompleteMove();
        }

        // 移動処理
        private void updatePosition(IntVect2D newPosition)
        {
            // 移動先のタイル位置
            var toTilePostion = BBattleStage.Instance.GetTileXAndZPosition(newPosition);
            // 向きの変更
            Vector2 targetPos = toTilePostion;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);

            // 座標変更
            var table = getMoveTable(toTilePostion);
            iTween.MoveTo(gameObject, table);

            // 配列値変更
            character.PositionArray = newPosition;
        }

        // ダメージを与える
        private void damageEndMove()
        {
            foreach(var target in TargetList) {
                var damageRate = calcMoveDamageRate();
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower, damageRate);
            }
        }

        // 移動テーブル作成
        private Hashtable getMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x", position.x);
            table.Add("z", position.y);
            table.Add("time", tileMoveTime);
            table.Add("easetype", iTween.EaseType.linear);
            return table;
        }

        private void onCompleteMove()
        {
            // カメラが上からみる→ダメージ演出→アクティブオン
            BCameraManager.Instance.ActiveUpMode();

            // ダメージ演出
            damageEndMove();
            //死のチェック
            foreach(var target in TargetList) {
                target.Life.CheckDestroy();
            }

            StartCoroutine(WaitTimer.WaitSecond(() => {
                onCompleteAction();
                IsNowAction = false;
                // カメラをリセットする
                BCameraManager.Instance.ActiveLeanMode();
                BCameraMove.Instance.MoveToBackForActive();
                //行動終了
                character.OnEndActive();
            }, 3f));
        }

        //倍率の算出
        private float calcMoveDamageRate()
        {
            //ベース＊技倍率
            return calcBaseDamageRate() * selectAttackParameter.powerRate;
        }

        void OnDestroy()
        {
            IT_Gesture.onDraggingStartE -= onDraggingStart;
        }
    }
}