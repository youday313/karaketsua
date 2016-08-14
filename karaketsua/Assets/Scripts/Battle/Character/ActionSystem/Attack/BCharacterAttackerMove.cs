using UnityEngine;
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
        private float tileMoveTime = 0.1f;

        //攻撃可能
        public bool IsAttackable {
            get {
                if(selectAttackParameter == null || IsSetTarget == false) return false;
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


            //ActionSelect.Instance.DisableMoveAttackButton();
            base.Disable();
        }
        //なぞり開始
        private void onDraggingStart(DragInfo dragInfo)
        {
            if(IsDone == true) return;
            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if(tilePosition == null) return;

            //キャラクターの上
            if(IntVect2D.IsEqual(tilePosition,character.positionArray) == false) return;
            BBattleStage.Instance.ResetAllTileColor();

            nowTraceTiles = new List<IntVect2D>();
            nowTraceTiles.Add(tilePosition);
            foreach(var target in attackTargetList) {
                target.SetTargeted(false);
            }
            attackTargetList = new List<BCharacterBase>();

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
            if(nowTraceTiles.Count == selectAttackParameter.moveRange) return;

            //タイル上
            var tilePosition = BBattleStage.Instance.GetTilePositionFromScreenPosition(dragInfo.pos);
            if(tilePosition == null) return;
            //まだ未通過
            if(IntVect2D.IsEqual(tilePosition,nowTraceTiles.LastOrDefault())) return;
            //まだ未完成
            if(selectAttackParameter.moveRange == nowTraceTiles.Count) return;

            //次のタイル
            if(IntVect2D.IsNeighbor(tilePosition,nowTraceTiles.LastOrDefault()) == false) return;

            nowTraceTiles.Add(tilePosition);
            BBattleStage.Instance.ChangeColor(tilePosition,TileState.Skill);

        }
        private void onDraggingEnd(DragInfo dragInfo)
        {

            checkTraceComplete();
            //nowTraceTiles = new List<IntVect2D>();
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
            if(BCharacterManager.Instance.IsExistCharacterOnTile(nowTraceTiles.Last()) == true) return;

            //成功

            setTarget();
            //攻撃範囲に敵がいない
            if(attackTargetList.Count == 0) {
                return;
            }

            //攻撃
            UIBottomCommandParent.UICommandState = EUICommandState.ExecuteAttack;
            UIBottomCommandParent.Instance.UpdateUI();
            //Attack();
            //Disable();
        }
        private void setTarget()
        {
            foreach(var traceTile in nowTraceTiles) {
                var target = BCharacterManager.Instance.GetOpponentCharacterOnTileFormVect2D(traceTile,character.isEnemy);
                if(target == null) continue;

                attackTargetList.Add(target);
                target.SetTargeted(true);
            }
        }

        //攻撃実行
        public void ExecuteAttack()
        {

            if(attackTargetList.Count == 0) return;

            ////攻撃
            //foreach (var target in attackTarget)
            //{
            //    target.Life.Damage(character.characterParameter.power);
            //    target.Life.CheckDestroy();
            //}

            //attackTarget = null;


            startMoveForAttack();
            IsDone = true;
            //攻撃時にUI非表示
            UIBottomCommandParent.UICommandState = EUICommandState.None;
            UIBottomAllManager.Instance.Off();

            //Disable();
        }
        private void startMoveForAttack()
        {
            animator.SetMoveAttack();
            IsNowAction = true;

            //コルーチンで移動
            StartCoroutine(move());
            attackMotionTime = 7f;
            Invoke("OnCompleteAnimation",attackMotionTime);
        }

        //移動コルーチン
        private IEnumerator move()
        {

            foreach(var trace in nowTraceTiles) {

                UpdatePosition(trace);
                DamageInMoving();
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
            iTween.MoveTo(gameObject,table);


            //配列値変更
            character.positionArray = newPosition;
        }
        //ダメージを与える
        void DamageInMoving()
        {

            //この関数内では攻撃後でもターゲットからはずさない、死のチェックは移動攻撃が終わった後

            var target = attackTargetList.Where(x => x.positionArray.IsEqual(character.positionArray)).FirstOrDefault();


            if(target != null) {

                var damageMagnification = CalcDamageMagnification();
                var characterPower = character.characterParameter.power;
                target.Life.Damage(characterPower,damageMagnification);
            }

        }

        Hashtable GetMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x",position.x);
            table.Add("z",position.y);
            //table.Add("time", 1.0f);
            table.Add("time",tileMoveTime);
            table.Add("easetype",iTween.EaseType.linear);
            return table;
        }


        void OnCompleteAnimation()
        {
            //死のチェック
            foreach(var target in attackTargetList) {
                target.Life.CheckDestroy();
            }
            attackTargetList = null;

            IsNowAction = false;
            BCameraManager.Instance.ActiveLeanMode();
            //行動終了
            character.OnEndActive();
        }
        //倍率の算出
        float CalcDamageMagnification()
        {
            //会心＊振れ幅＊技倍率＊タップ倍率
            return 1 * 1 * selectAttackParameter.powerMagnification * 1;
        }



    }
}