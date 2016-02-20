using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//移動攻撃
public class CharacterMoveAttack : CharacterAttacker {

    //移動範囲
    public int moveRange;
    //現在なぞっている範囲
    List<IntVect2D> nowTraceTiles = new List<IntVect2D>();
    [System.NonSerialized]
    public bool isNowCharge = false;
    GameObject startAttackPanel;

    public override void Enable()
    {
        base.Enable();
        //IT_Gesture.onTouchDownE+=OnTouchDown;
        //IT_Gesture.onMouse1DownE += OnMouseDown;
        //IT_Gesture.onShortTapE += OnShortTap;
        //BattleStage.Instance.UpdateTileColors(this.character, TileState.Skill);
        SetUpCamera();
        IT_Gesture.onDraggingStartE += OnDraggingStart;

    }
    public override void Disable()
    {
        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        //BattleStage.Instance.ResetAllTileColor();
        IT_Gesture.onDraggingStartE -= OnDraggingStart;
        ActionSelect.Instance.DisableMoveAttackButton();
        base.Disable();
    }
    void SetUpCamera()
    {
        CameraChange.Instance.ChangeCameraModeForMoveAttack();
    }
    //なぞり開始
    void OnDraggingStart(DragInfo dragInfo)
    {
        isNowAttackable = false;
        //タイル上
        var tilePosition = TileBase.GetArrayFromRay(dragInfo.pos);
        if (tilePosition == null) return;

        //キャラクターの上
        if(IntVect2D.IsEqual(tilePosition,character.positionArray)==false)return;
        BattleStage.Instance.ResetAllTileColor();

        nowTraceTiles = new List<IntVect2D>();
        nowTraceTiles.Add(tilePosition);
        BattleStage.Instance.ChangeColor(tilePosition, TileState.Skill);

        IT_Gesture.onDraggingE += OnDragging;
        IT_Gesture.onDraggingEndE += OnDraggingEnd;
        isNowCharge = true;
        ActionSelect.Instance.DisableMoveAttackButton();
    }
    void OnDragging(DragInfo dragInfo)
    {
        //もうなぞれない
        if (nowTraceTiles.Count == moveRange) return;

        //タイル上
        var tilePosition = TileBase.GetArrayFromRay(dragInfo.pos);
        if (tilePosition == null) return;
        //まだ未通過
        if (IntVect2D.IsEqual(tilePosition, nowTraceTiles.LastOrDefault())) return;
        //まだ未完成
        if (moveRange == nowTraceTiles.Count) return;

        //次のタイル
        if (IntVect2D.IsNeighbor(tilePosition, nowTraceTiles.LastOrDefault()) == false) return;

        nowTraceTiles.Add(tilePosition);
        BattleStage.Instance.ChangeColor(tilePosition, TileState.Skill);

    }
    void OnDraggingEnd(DragInfo dragInfo)
    {

        CheckTraceComplete();
        //nowTraceTiles = new List<IntVect2D>();
        IT_Gesture.onDraggingE -= OnDragging;
        isNowCharge = false;
        IT_Gesture.onDraggingEndE -= OnDraggingEnd;
    }
    //移動攻撃可能になった
    bool isNowAttackable = false;
    void CheckTraceComplete()
    {
        //BattleStage.Instance.ResetAllTileColor();
        
        //距離をなぞった
        //失敗
        if (nowTraceTiles.Count != moveRange)
        {
            foreach (var tile in nowTraceTiles)
            {
                BattleStage.Instance.ResetAllTileColor();
            }
            return;
        }
        //成功

        SetTarget();
        //攻撃範囲に敵がいない
        if (attackTarget.Count == 0) return;
        Debug.Log("AA");
        //攻撃
        isNowAttackable = true;
        ActionSelect.Instance.EnableMoveAttackButton();
        //Attack();
        //Disable();
    }
    void SetTarget()
    {
        foreach (var traceTile in nowTraceTiles)
        {
            var target = GetOpponentCharacterOnTile(traceTile);
            if (target == null) continue;
            attackTarget.Add(target);
        }
    }
    void Attack()
    {

        if (attackTarget.Count == 0) return;
        //攻撃
        foreach (var target in attackTarget)
        {
            target.Damage(character.characterParameter.power);
            target.CheckDestroy();
        }

        attackTarget = null;


        StartAttackAnimation();
        //攻撃時にUI非表示
        Disable();
        ActionSelect.Instance.EndActiveAction();
        ActionSelect.Instance.DisableMoveAttackButton();
    }
    void StartAttackAnimation()
    {

        animator.SetTrigger("TraceAttack");
        isNowAction = true;
        attackMotionTime = 7f;
        Invoke("OnCompleteAnimation", attackMotionTime);
        //cameraMove.MoveToAttack(this, attackTarget[0].transform.position);

    }
    //攻撃モーション時間
    //モーション時間＋猶予時間の案もありか
    public float attackMotionTime = 3f;
    void OnCompleteAnimation()
    {
        isNowAction = false;
        CameraChange.Instance.ChangeCameraMode(CameraMode.FromBack);
        character.EndActiveCharacterAction();
    }

    //攻撃開始
    //characterから呼ばれる
    public void ExcuteAttack(){
        Attack();
    }

}
