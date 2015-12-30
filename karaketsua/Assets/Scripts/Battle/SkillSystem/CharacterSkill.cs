using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CharacterSkill : CharacterAttacker
{

    public List<IntVect2D> skillTiles;
    List<IntVect2D> nowTraceTiles = new List<IntVect2D>();
    [System.NonSerialized]
    public bool isNowCharge = false;

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
        base.Disable();
    }
    void SetUpCamera()
    {
        CameraChange.Instance.ChangeCameraMode(CameraMode.Up);
    }

    //なぞり開始
    void OnDraggingStart(DragInfo dragInfo)
    {
        //タイル上
        var targetPosition = TileBase.GetArrayFromRay(dragInfo.pos);
        if (targetPosition == null) return;


        nowTraceTiles = new List<IntVect2D>();
        nowTraceTiles.Add(targetPosition);
        BattleStage.Instance.ChangeColor(targetPosition, TileState.Skill);

        IT_Gesture.onDraggingE += OnDragging;
        IT_Gesture.onDraggingEndE += OnDraggingEnd;
        isNowCharge = true;
    }

    void OnDragging(DragInfo dragInfo)
    {
        //タイル上
        var targetPosition = TileBase.GetArrayFromRay(dragInfo.pos);
        if (targetPosition == null) return;
        //まだ未通過
        if (IntVect2D.IsEqual(targetPosition, nowTraceTiles.LastOrDefault())) return;
        //まだ未完成
        if (skillTiles.Count == nowTraceTiles.Count) return;

        //次のタイル
        if (IntVect2D.IsEqual(targetPosition, IntVect2D.Add(skillTiles[nowTraceTiles.Count], nowTraceTiles[0])) == false)
        {
            FailAciton();
            return;
        }

        nowTraceTiles.Add(targetPosition);
        BattleStage.Instance.ChangeColor(targetPosition,TileState.Skill);

    }

    void OnDraggingEnd(DragInfo dragInfo)
    {

        CheckTraceComplete();
        nowTraceTiles = new List<IntVect2D>();
        IT_Gesture.onDraggingE -= OnDragging;
        isNowCharge = false;
        IT_Gesture.onDraggingEndE -= OnDraggingEnd;
    }
    void FailAciton()
    {
        BattleStage.Instance.ResetAllTileColor();
        nowTraceTiles = new List<IntVect2D>();
        IT_Gesture.onDraggingE -= OnDragging;
        isNowCharge = true;
    }
    void CheckTraceComplete()
    {

        BattleStage.Instance.ResetAllTileColor();
        //完全になぞった
        if (nowTraceTiles.Count != skillTiles.Count) return;

        SetTarget();
        //攻撃範囲に敵がいない
        if (attackTarget.Count == 0) return;

        //攻撃

        Attack();
        Disable();
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
            target.ExecuteAttack();
        }
        
        StartAttackAnimation();
        //攻撃時にUI非表示
        ActionSelect.Instance.EndActiveAction();
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


    IEnumerator ICreateNewWave()
    {
        //foreach (var wave in skillTileWaves)
        //{
        //  CreateNewWave(wave);

        yield return null;
        //}
    }
    void CreateNewWave(SkillTileWave wave)
    {
        wave.CreateNewTileSequence(character.positionArray);
    }

    public void SuccessWave()
    {

    }
}
