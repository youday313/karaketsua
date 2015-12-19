using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CharacterAttacker : MonoBehaviour {

    protected Character character;
        [System.NonSerialized]
    public bool isNowAction = false;
        [System.NonSerialized]
    public bool isSetTarget = false;
    bool isEnable = false;
    public bool IsEnable
    {
        get { return isEnable; }
        set
        {
            if (isEnable == false&&value==true)
            {
                Enable();
            }
            else if(isEnable==true&&value==false)
            {
                Disable();
            }
            isEnable = value;
        }

    }
    protected Animator animator;

    List<Character> attackTarget=new List<Character>();
    CameraMove cameraMove;
    //選択した攻撃方法
    [System.NonSerialized]
    public AttackParameter selectAttackParameter=null;

	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
	}

    void OnActiveCharacter()
    {
        isSetTarget = false;
        isNowAction = false;
    }

    void Enable()
    {
        OnActiveCharacter();
        //IT_Gesture.onTouchDownE+=OnTouchDown;
        //IT_Gesture.onMouse1DownE += OnMouseDown;
        IT_Gesture.onShortTapE += OnShortTap;
        //BattleStage.Instance.UpdateTileColors(this.character, TileState.Attack);
        
        //仮で必ず最初の攻撃方法を取る
        selectAttackParameter = character.characterParameter.attackParameter[0];

		BattleStage.Instance.ChangeTileColorsToAttack(selectAttackParameter.attackRange,this.character);
    }
    void Disable()
    {
        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        IT_Gesture.onShortTapE -= OnShortTap;
        BattleStage.Instance.ResetAllTileColor();
    }

    void OnShortTap(Vector2 pos)
    {
        UpdateAttackState(pos);

    }
    
    public void UpdateAttackState(Vector2 position)
    {
        if (isNowAction == true) return;
        if (CameraChange.Instance.nowCameraMode != CameraMode.FromBack && CameraChange.Instance.nowCameraMode != CameraMode.FromFront) return;

        SetTarget(position);
        //if (isSetTarget == false)
        //{
        //    SetTarget(position);
        //}
        //else
        //{
        //    Attack(position);
        //}
    }
    //ターゲット選択ボタンを選択した時
    void SetAttackMode(bool isSet)
    {
        isSetTarget = isSet;
        ActionSelect.Instance.EnableAttackButton();

    }

    #region::ターゲット選択
    //ターゲットの決定
    public void SetTarget(Vector2 touchPosition)
    {
        //ターゲットの検索
        var target = GetOpponentCharacterFromTouch(touchPosition);

        //ターゲットが存在しないマスをタップ
        if (target == null) return;

        //攻撃範囲内
        //if (Mathf.Abs(target.positionArray.x - character.positionArray.x) + Mathf.Abs(target.positionArray.y - character.positionArray.y) > character.characterParameter.attackRange) return;
        if (IsInAttackRange(target.positionArray)==false) return;
        
        //複数攻撃
        if (selectAttackParameter.isMultiAttack == true)
        {
            //未選択からの選択
            if (attackTarget.Contains(target) == false)
            {
                //タイル変更
 
                attackTarget.Add(target);
            }
                //選択からの解除
            else
            {
                //タイル変更
                attackTarget.Remove(target);
                return;
            }
            
        }
        else
        {
            //他のキャラが既に選択されていた場合は除く
            if (attackTarget.Count != 0)
            {
                //タイル変更
                //除く
                attackTarget = new List<Character>();
            }
            //再設定
            attackTarget.Add(target);
            
        }
        //ターゲットのタイル変更
        //BattleStage.Instance.UpdateTileColors(target, TileState.Attack);


        SetAttackMode(true);
        //PlayerOwner.Instance.commandState = CommandState.Attack;

    }

    bool IsInAttackRange(IntVect2D targetPositionArray)
    {
        return selectAttackParameter.attackRange.Any(x=>x.IsEqual(IntVect2D.Sub(targetPositionArray,character.positionArray)));
    }


    #endregion::ターゲット選択

    #region::攻撃
    //ターゲットが選択されていた時タイルタップ
    public void Attack()
    {
        if (attackTarget.Count == 0) return;
        Debug.Log("Ina");
        //攻撃
        foreach (var target in attackTarget)
        {
            target.Damage(character.characterParameter.power);
        }
        StartAttackAnimation();
        //攻撃時にUI非表示
        ActionSelect.Instance.EndActiveAction();

    }

    //旧版
    //public void Attack(Vector2 touchPosition)
    //{
    //    //ターゲットの検索
    //    var target = GetOpponentCharacterFromTouch(touchPosition);

    //    //ターゲットが存在しないマスをタップ
    //    if (target == null)
    //    {
    //        //PlayerOwner.Instance.SetCommandState(CommandState.TargetSelect);
    //        SetAttackMode(true);
    //        //ターゲットのタイル変更
    //        return;

    //    }
    //    //ターゲット以外のキャラをタップ
    //    //ターゲットの切り替え
    //    else if (target != attackTarget)
    //    {
    //        //BattleStage.Instance.ChangeNeighborTilesColor(positionArray, TileState.Attack);
    //        SetTarget(touchPosition);
    //    }

    //    //攻撃
    //    target.Damage(character.characterParameter.power);
    //    StartAttackAnimation();
    //    //攻撃時にUI非表示
    //    ActionSelect.Instance.EndActiveAction();
    //}
    //攻撃モーション時間
    //モーション時間＋猶予時間の案もありか
    public float attackMotionTime=3f;
    void StartAttackAnimation()
    {

        animator.SetTrigger("Attack");
        isNowAction=true;
        Invoke("OnCompleteAnimation",attackMotionTime);
        cameraMove.MoveToAttack(this, attackTarget[0].transform.position);

    }
    void OnCompleteAnimation()
    {
        isNowAction = false;
        character.ResetActive();
    }

    #endregion::攻撃


    //タイル上のキャラが自身にとっての敵キャラなら取得
    Character GetOpponentCharacterOnTile(IntVect2D toPos)
    {

        var chara = Character.GetCharacterOnTile(toPos);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;
    }
    Character GetOpponentCharacterFromTouch(Vector2 touchPosition)
    {
        var chara = Character.GetCharacterOnTile(touchPosition);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;

    }
}
