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

	// Use this for initialization
	void Start () {
        character=GetComponent<Character>();
        animator = GetComponent<Animator>();
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
        IT_Gesture.onDraggingEndE += OnDragMove;
        IT_Gesture.onChargeStartE += ActiveSelectMoveCursor;
        IT_Gesture.onChargeEndE += DisactiveSelectMoveCursor;


    }
    public void Disable(){
        IT_Gesture.onDraggingEndE-=OnDragMove;
        IT_Gesture.onChargeStartE -= ActiveSelectMoveCursor;
        IT_Gesture.onChargeEndE -= DisactiveSelectMoveCursor;
        directionIcon.SetActive(false);
    }
    void OnDragMove(DragInfo dragInfo)
    {

        RequestMove(dragInfo.delta);

    }
    //一連の移動判定処理の開始
    void RequestMove(Vector2 delta)
    {
        if(CanMoveFromState()==false)return;

        var toVect = GetMoveDirection(delta);
        var newPosition = new IntVect2D(character.positionArray);
        newPosition.x = Mathf.Clamp(character.positionArray.x + toVect.x, -BattleStage.stageSizeX, BattleStage.stageSizeX);
        newPosition.y = Mathf.Clamp(character.positionArray.y + toVect.y, -BattleStage.stageSizeY, BattleStage.stageSizeY);

        if(CanMoveFromTileState(newPosition)==false)return;

        //移動実行
        UpdatePosition(newPosition);

        UpdateAnimation();

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
        //配列値変更
        character.positionArray = newPosition;

        //移動先のタイル位置
        var toTilePostion = BattleStage.Instance.GetTileXAndZPosition(newPosition);
        var table = SetMoveTable(toTilePostion);
        //座標変更
        iTween.MoveTo(gameObject, table);

        isMoved = true;
        isNowAction = true;

        directionIcon.SetActive(false);

        UpdateAnimation();
    }
    void UpdateTileState()
    {

        //自分の乗っているタイルの色変更
        //BattleStage.Instance.UpdateTileColors(positionArray, TileState.Moved);
    }
    //アニメーション状態
    void UpdateAnimation()
    {
        if (isNowAction == true)
        {
            animator.SetBool("Move", true);
        }
        else if (isNowAction == false)
        {
            animator.SetBool("Move", false);
        }
    }
    //移動アニメーション作成
    Hashtable SetMoveTable(Vector2 position)
    {
        Hashtable table = new Hashtable();
        table.Add("x", position.x);
        table.Add("z", position.y);
        //table.Add("time", 1.0f);
        table.Add("time", 1.6);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        return table;
    }
    void CompleteMove()
    {
        isNowAction = false;
        UpdateAnimation();
        //character.ResetActive();
    }

    //ホールド時矢印を出す
    void ActiveSelectMoveCursor(ChargedInfo cInfo)
    {

    }
    void DisactiveSelectMoveCursor(ChargedInfo cInfo)
    {

    }
}
