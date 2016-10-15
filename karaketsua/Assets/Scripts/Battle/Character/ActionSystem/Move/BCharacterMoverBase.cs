using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterMoverBase : MonoBehaviour
    {

        protected BCharacterBase character;
        protected BCharacterAnimator animator;

        //行動可能時に有効化
        protected bool isEnable = false;
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                if (value == true)
                {
                    Enable();
                }
                else if (value == false)
                {
                    Disable();
                }
                isEnable = value;
            }
        }
        [System.NonSerialized]
        protected bool isNowAction = false;
        public bool IsNowAction
        {
            get { return isNowAction; }
        }
        [System.NonSerialized]
        protected bool isDone = false;
        public bool IsDone
        {
            get { return isDone; }
        }



        // Use this for initialization
        public virtual void Awake()
        {
            character = GetComponent<BCharacterBase>();
            animator = GetComponent<BCharacterAnimator>();
        }

        public virtual void Enable()
        {

        }
        public virtual void Disable()
        {
        }


        public virtual void Reset()
        {
            isDone = false;
            isNowAction = false;

        }



        //移動可能距離
        [System.NonSerialized]
        public int movableCount = 1;



        //一連の移動判定処理の開始
        public void RequestMoveFromVect2D(IntVect2D toVect2D)
        {

            //新しいタイルポジション
            var newPosition = new IntVect2D(
                Mathf.Clamp(character.PositionArray.x + toVect2D.x, -BBattleStage.stageSizeX, BBattleStage.stageSizeX),
                Mathf.Clamp(character.PositionArray.y + toVect2D.y, -BBattleStage.stageSizeY, BBattleStage.stageSizeY));

            //移動先が空いている
            if (BCharacterManager.Instance.IsExistCharacterOnTile(newPosition) == true) return;

            //移動実行
            UpdatePosition(newPosition);

        }

        //実際に移動
        protected void UpdatePosition(IntVect2D newPosition)
        {
            //移動方向
            var direction = IntVect2D.GetDirection(character.PositionArray, newPosition);
            //移動先のタイル位置
            var toTilePostion = BBattleStage.Instance.GetTileXAndZPosition(newPosition);
            var table = GetMoveTable(toTilePostion);

            //現在のタイルの実座標
            var oldTilePosition = BBattleStage.Instance.GetTileXAndZPosition(character.PositionArray);
            //カメラをアップデート
            BCameraMove.Instance.FollowCharacter(toTilePostion - oldTilePosition, animator.moveTime);

            //座標変更
            iTween.MoveTo(gameObject, table);

            isDone = true;
            isNowAction = true;

            //配列値変更
            character.PositionArray = newPosition;

            StartRotateAnimation(direction);
            StartAnimation();


            UpdateInManualState();
            //UI
            UIBottomCommandParent.UICommandState = EUICommandState.Action;
            UIBottomAllManager.Instance.UpdateUI();
        }

        Hashtable GetMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x", position.x);
            table.Add("z", position.y);
            table.Add("time", animator.moveTime);
            table.Add("easetype", iTween.EaseType.linear);
            table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
            return table;
        }

        void StartRotateAnimation(IntVect2D vect)
        {
            float angleX = vect.x * 90;
            float angleY = Mathf.Sign(vect.y) > 0 ? 0 : 180;
            float isEnemyAngle = character.IsEnemy == false ? 1 : -1;
            transform.eulerAngles = new Vector3(0, angleX + isEnemyAngle * angleY, 0);
        }

        //手動移動のみ使用
        public virtual void UpdateInManualState()
        {

        }
        public virtual void CompleteMove()
        {
            isNowAction = false;
            StopAnimation();
        }

        void StartAnimation()
        {
            animator.SetMove(true);
        }
        public void StopAnimation()
        {
            animator.SetMove(false);
            character.ResetRotate();
        }
    }
}