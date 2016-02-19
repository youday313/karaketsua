using UnityEngine;
using System.Collections;

using BattleScene;

namespace BattleScene
{

    public class BCharacterMove : BCharacterBaseAction
    {
        //移動可能距離
        [System.NonSerialized]
        public int movableCount = 1;
        //既に移動したか
        [System.NonSerialized]
        public bool isMoved = false;
        //移動方向のオブジェクト
        public GameObject directionIcon;

        CameraMove cameraMove;
        
        public override void Init()
        {
            base.Init();

        }
        //選択可能時
        public override void Enable()
        {
            base.Enable();
            if (isMoved == true)
            {
                //::タイルの色変更

                return;
            }
            cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();


            //OnActiveCharacter();
            //キャラクター移動選択
            IT_Gesture.onDraggingStartE += OnChargeForMove;

            directionIcon.SetActive(true);
            //BattleStage.Instance.UpdateTileColors(this.character, TileState.Move);


        }
        //選択不可能時、行動は可能
        public override void Disable()
        {
            //キャラクター移動選択
            IT_Gesture.onDraggingStartE -= OnChargeForMove;
            IT_Gesture.onDraggingEndE -= OnDragMove;
            directionIcon.SetActive(false);
            base.Disable();
        }
        //行動終了時
        public override void Reset()
        {
            base.Reset();
            isMoved = false;
            IsEnable = false;
        }

        //スワイプによる移動操作検知
        void OnChargeForMove(DragInfo dragInfo)
        {
            //自分キャラ
            if (Character.GetCharacterOnTile(dragInfo.pos) != this.character) return;

            //キャラクター移動用
            IT_Gesture.onDraggingEndE += OnDragMove;
        }
        //スワイプによる移動操作終了
        void OnDragMove(DragInfo dragInfo)
        {
            //移動方向決定
            RequestMove(dragInfo.delta);
            IT_Gesture.onDraggingEndE -= OnDragMove;
        }

        //一連の移動判定処理の開始
        void RequestMove(Vector2 delta)
        {

            //カメラから方向取得
            var toVect = cameraMove.GetMoveDirection(delta);
            //Vect2D化
            var toVect2D = IntVect2D.GetDirectionFromVector2(toVect);

            //新しいタイルポジション
            var newPosition = new IntVect2D(
                Mathf.Clamp(character.positionArray.x + toVect2D.x, -BattleStage.stageSizeX, BattleStage.stageSizeX),
                Mathf.Clamp(character.positionArray.y + toVect2D.y, -BattleStage.stageSizeY, BattleStage.stageSizeY));
            
            //ステージが移動可能
            if (CharacterManager.Instance.IsExistCharacterOnTile(newPosition)==false) return;

            //移動実行
            UpdatePosition(newPosition);

            StartAnimation();

            //コマンド変更
            //character.characterState = CharacterState.Wait;
        }

        //実際に移動
        void UpdatePosition(IntVect2D newPosition)
        {
            //移動方向
            var direction = IntVect2D.GetDirection(character.positionArray, newPosition);
            //現在のタイルの実座標
            var oldTilePosition = BattleStage.Instance.GetTileXAndZPosition(character.positionArray);

            //移動先のタイル位置
            var toTilePostion = BattleStage.Instance.GetTileXAndZPosition(newPosition);
            var table = GetMoveTable(toTilePostion);

            //座標変更
            iTween.MoveTo(gameObject, table);

           //カメラをアップデート
            cameraMove.FollowCharacter(toTilePostion - oldTilePosition, animator.moveTime);
            
            isMoved = true;
            isNowAction = true;

            //配列値変更
            character.positionArray = newPosition;

            directionIcon.SetActive(false);
            StartRotateAnimation(direction);

        }
        
        Hashtable GetMoveTable(Vector2 position)
        {
            Hashtable table = new Hashtable();
            table.Add("x", position.x);
            table.Add("z", position.y);
            //table.Add("time", 1.0f);
            table.Add("time", animator.moveTime);
            table.Add("easetype", iTween.EaseType.linear);
            table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
            return table;
        }
        void CompleteMove()
        {
            isNowAction = false;
            StopAnimation();
            BattleStage.Instance.ResetAllTileColor();
        }
        void StopAnimation()
        {
            animator.SetMove(false);
            ResetRoatateAnimation();
        }
        void ResetRoatateAnimation()
        {
            var isEnemyAngle = character.isEnemy == true ? 180 : 0;
            transform.eulerAngles = new Vector3(0, isEnemyAngle, 0);

        }
        void StartRotateAnimation(IntVect2D vect)
        {
            float angleX = vect.x * 90;
            float angleY = Mathf.Sign(vect.y) > 0 ? 0 : 180;
            float isEnemyAngle = character.isEnemy == false ? 1 : -1;
            transform.eulerAngles = new Vector3(0, angleX + isEnemyAngle * angleY, 0);
        }
        void StartAnimation()
        {
            animator.SetMove(true);
        }


    }
}