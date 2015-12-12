using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//キャラクターの移動に関するクラス

public class CharacterMover : MonoBehaviour {

    Character character;

    [System.NonSerialized]
    public int movableCount = 1;//移動可能距離
        [System.NonSerialized]
    bool isMoved = false;
        [System.NonSerialized]
    public bool isNowAction = false;
    Animator animator;
    bool isEnable = false;
    public GameObject directionIcon;

    CameraMove cameraMove;
	// Use this for initialization
	void Start () {
        character=GetComponent<Character>();
        animator = GetComponent<Animator>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
	}

    public void OnActiveCharacter()
    {
        isMoved = false;
        directionIcon.SetActive(true);
        Enable();
    }

    
    public void Enable()
    {
        
        isNowAction = false;
        
        //キャラクター移動選択
        IT_Gesture.onDraggingStartE += OnChargeForMove;
        //キャラクター移動用
        IT_Gesture.onDraggingEndE += OnDragMove;
        //カーソル表示用
        //IT_Gesture.onChargeStartE += ActiveSelectMoveCursor;
        //IT_Gesture.onChargeEndE += DisactiveSelectMoveCursor;
        if (isMoved == false)
        {
            directionIcon.SetActive(true);
            BattleStage.Instance.UpdateTileColors(this.character, TileState.Move);
        }

    }
    public void Disable(){
        //キャラクター移動選択
        IT_Gesture.onDraggingStartE += OnChargeForMove;
        IT_Gesture.onDraggingEndE-=OnDragMove;
        //IT_Gesture.onChargeStartE -= ActiveSelectMoveCursor;
        //IT_Gesture.onChargeEndE -= DisactiveSelectMoveCursor;
        directionIcon.SetActive(false);
    }
    //移動のための選択
    public bool isNowCharge = false;
    void OnChargeForMove(DragInfo dragInfo)
    {
        //移動可能
        if (CanMoveFromState() == false) return;
        //自分キャラ
        if (Character.GetCharacterOnTile(dragInfo.pos) != this.character) return;

        isNowCharge = true;
    }
    void OnDragMove(DragInfo dragInfo)
    {
        if (isNowCharge == true)
        {
            RequestMove(dragInfo.delta);
        }

        isNowCharge = false;


    }
    //一連の移動判定処理の開始
    void RequestMove(Vector2 delta)
    {
        if(CanMoveFromState()==false)return;
        //カメラから方向取得
        var toVect = cameraMove.GetMoveDirection(delta);
        var toVect2D = GetMoveDirection(toVect);
        //var toVect = GetMoveDirection(delta);
        var newPosition = new IntVect2D(character.positionArray);
        newPosition.x = Mathf.Clamp(character.positionArray.x + toVect2D.x, -BattleStage.stageSizeX, BattleStage.stageSizeX);
        newPosition.y = Mathf.Clamp(character.positionArray.y + toVect2D.y, -BattleStage.stageSizeY, BattleStage.stageSizeY);

        if(CanMoveFromTileState(newPosition)==false)return;

        //移動実行
        UpdatePosition(newPosition);

        StartAnimation();

        UpdateTileState();
        //コマンド変更
        character.characterState = CharacterState.Wait;
    }

    //移動可能か
    bool CanMoveFromState()
    {
        //選択されている
        if (character.isNowSelect == false) return false;
        //State判定
        if (character.characterState != CharacterState.Wait) return false;
        //既に移動していない
        if (isMoved == true) return false;
        return true;
    }
    //どの方向に動くか
    IntVect2D GetMoveDirection(Vector2 delta)
    {
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return new IntVect2D((int)Mathf.Sign(delta.x), 0);

        }
        //y方向
        else
        {
            return new IntVect2D(0, (int)Mathf.Sign(delta.y));
        }
    }

    //タイルの状態による移動可能判定
    bool CanMoveFromTileState(IntVect2D newPosition)
    {
        //移動先のタイル
        var toTile = BattleStage.Instance.GetTile(newPosition);
        //タイルが存在しない
        if (toTile == null) return false;
        //既にキャラが居る
        if (IsExistCharacterOnTile(newPosition) == true) return false;
        return true;
    }
    //移動先にキャラが既にいない
    bool IsExistCharacterOnTile(IntVect2D toPos)
    {

        var target = CharacterManager.Instance.characters.
            Where(t => toPos.IsEqual(t.positionArray)).
            FirstOrDefault();
        return target != null;
    }
    //実際に移動
    void UpdatePosition(IntVect2D newPosition)
    {
        //移動方向
        var direction = GetDirection(character.positionArray, newPosition);
        var oldTilePosition = BattleStage.Instance.GetTileXAndZPosition(character.positionArray);
        //配列値変更
        character.positionArray = newPosition;

        //移動先のタイル位置
        var toTilePostion = BattleStage.Instance.GetTileXAndZPosition(newPosition);
        var table = SetMoveTable(toTilePostion);

        //座標変更
        iTween.MoveTo(gameObject, table);

        var realCameraMovePosition = toTilePostion - oldTilePosition;
        cameraMove.FollowCharacter(realCameraMovePosition, moveTime);
        isMoved = true;
        isNowAction = true;

        directionIcon.SetActive(false);

        StartAnimation();
        StartRotateAnimation(direction);
    }
    IntVect2D GetDirection(IntVect2D oldVect,IntVect2D newVect)
    {
        return new IntVect2D(newVect.x - oldVect.x, newVect.y-oldVect.y);
    }
    void UpdateTileState()
    {

        //自分の乗っているタイルの色変更
        //BattleStage.Instance.UpdateTileColors(positionArray, TileState.Moved);
    }
    //アニメーション状態
    //void UpdateAnimation()
    //{
    //    if (isNowAction == true)
    //    {
    //        animator.SetBool("Move", true);
    //        StartRotateAnimation();
    //    }
    //    else if (isNowAction == false)
    //    {
    //        animator.SetBool("Move", false);
            
    //    }
    //}
    void StartAnimation()
    {
        animator.SetBool("Move", true);
        
    }
    void StopAnimation()
    {
        animator.SetBool("Move", false);
        ResetRoatateAnimation();
    }
    void StartRotateAnimation(IntVect2D vect)
    {
        float angleX = vect.x * 90;
        float angleY = Mathf.Sign(vect.y) > 0 ? 0 : 180;
        float isEnemyAngle = character.isEnemy == false ? 1 : -1;
        transform.eulerAngles = new Vector3(0, angleX + isEnemyAngle * angleY, 0);
        
    }
    void ResetRoatateAnimation()
    {
        var isEnemyAngle = character.isEnemy == true ? 180 : 0;
        transform.eulerAngles = new Vector3(0, isEnemyAngle, 0);
    }
    //移動アニメーション作成
    float moveTime = 1.6f;
    Hashtable SetMoveTable(Vector2 position)
    {
        Hashtable table = new Hashtable();
        table.Add("x", position.x);
        table.Add("z", position.y);
        //table.Add("time", 1.0f);
        table.Add("time", moveTime);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        return table;
    }
    void CompleteMove()
    {
        isNowAction = false;
        StopAnimation();
        BattleStage.Instance.ResetTileColor();
        //character.ResetActive();
    }

    //ホールド時矢印を出す
    void ActiveSelectMoveCursor(DragInfo cInfo)
    {
       
    }
    void DisactiveSelectMoveCursor(ChargedInfo cInfo)
    {

    }
}
