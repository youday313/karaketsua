using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterMoverManual : BCharacterMoverBase
    {
        //移動方向のオブジェクト
        GameObject directionIcon;

        public override void Awake()
        {
            base.Awake();
            //矢印マーカー表示
            var obj = Instantiate(Resources.Load<GameObject>("DirectionIcons"));
            obj.transform.SetParent(this.gameObject.transform);
            directionIcon = obj;
            directionIcon.SetActive(false);
        }

        //選択可能時
        public override void Enable()
        {
            if (isDone == true) return;
            base.Enable();
            //OnActiveCharacter();
            //キャラクター移動選択
            IT_Gesture.onDraggingStartE += OnChargeForMove;
            directionIcon.SetActive(true);
            BBattleStage.Instance.OnMoverable(this.character.PositionArray);
        }
        

        public override void Disable()
        {
            //キャラクター移動選択
            Debug.Log("OnC");
            IT_Gesture.onDraggingStartE -= OnChargeForMove;
            IT_Gesture.onDraggingEndE -= OnDragMove;
            directionIcon.SetActive(false);
            BBattleStage.Instance.ResetAllTileColor();
        }
        
        public override void Reset()
        {
            Disable();

            base.Reset();
        }

        //スワイプによる移動操作検知
        void OnChargeForMove(DragInfo dragInfo)
        {
            //自分キャラ
            if (BCharacterManager.Instance.GetCharacterOnTile(dragInfo.pos) != this.character) return;

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
            var toVect = BCameraMove.Instance.GetMoveDirection(delta);
            //Vect2D化
            var toVect2D = IntVect2D.GetDirectionFromVector2(toVect);

            RequestMoveFromVect2D(toVect2D);
        }

        //移動行動を受け付けなくする
        public override void UpdateInManualState()
        {
            IT_Gesture.onDraggingStartE -= OnChargeForMove;
            directionIcon.SetActive(false);
            BBattleStage.Instance.ResetAllTileColor();
        }



        public override void CompleteMove()
        {
            base.CompleteMove();

            BBattleStage.Instance.ResetAllTileColor();

            UIBottomCommandParent.UICommandState = EUICommandState.Action;
            UIBottomAllManager.Instance.UpdateUI();
        }

        void OnDestory()
        {
            IT_Gesture.onDraggingStartE -= OnChargeForMove;
        }
        
    }
}