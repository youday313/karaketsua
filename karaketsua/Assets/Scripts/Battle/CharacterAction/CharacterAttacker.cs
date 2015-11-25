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

    Character attackTarget;
	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
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
        CameraMove.Instance.SetAttackMoveMode(true);
        BattleStage.Instance.UpdateTileColors(this.character, TileState.Attack);
    }
    void Disable()
    {
        //IT_Gesture.onTouchDownE -= OnTouchDown;
        //IT_Gesture.onMouse1DownE -= OnMouseDown;
        IT_Gesture.onShortTapE -= OnShortTap;
        CameraMove.Instance.SetAttackMoveMode(false);
        BattleStage.Instance.ResetTileColor();
    }

    void OnShortTap(Vector2 pos)
    {
        UpdateAttackState(pos);

    }
    
    public void UpdateAttackState(Vector2 position)
    {
        if (isNowAction == true) return;
        if (isSetTarget == false)
        {
            SetTarget(position);
        }
        else
        {
            Attack(position);
        }
    }
    void SetAttackMode(bool isSet)
    {
        isSetTarget = isSet;

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
        if (Mathf.Abs(target.positionArray.x - character.positionArray.x) + Mathf.Abs(target.positionArray.y - character.positionArray.y) > character.characterParameter.attackRange) return;
     

        attackTarget = target;
        //ターゲットのタイル変更
        //BattleStage.Instance.UpdateTileColors(target, TileState.Attack);


        SetAttackMode(true);
        //PlayerOwner.Instance.commandState = CommandState.Attack;

    }

    //ターゲット選択ボタンを選択した時
    public void SetAttackMode()
    {
        //BattleStage.Instance.UpdateTileColors(positionArray, TileState.Attackable);


    }
    #endregion::ターゲット選択

    #region::攻撃
    //ターゲットが選択されていた時タイルタップ
    public void Attack(Vector2 touchPosition)
    {
        //ターゲットの検索
        var target = GetOpponentCharacterFromTouch(touchPosition);

        //ターゲットが存在しないマスをタップ
        if (target == null)
        {
            //PlayerOwner.Instance.SetCommandState(CommandState.TargetSelect);
            SetAttackMode(true);
            //ターゲットのタイル変更
            return;

        }
        //ターゲット以外のキャラをタップ
        //ターゲットの切り替え
        else if (target != attackTarget)
        {
            //BattleStage.Instance.ChangeNeighborTilesColor(positionArray, TileState.Attack);
            SetTarget(touchPosition);
        }

        //攻撃
        target.Damage(character.characterParameter.power);
        StartAttackAnimation();
    }
    //攻撃モーション時間
    //モーション時間＋猶予時間の案もありか
    public float attackMotionTime=1f;
    void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
        isNowAction=true;
        Invoke("OnCompleteAnimation",attackMotionTime);
    }
    void OnCompleteAnimation()
    {
        isNowAction = false;
        character.ResetActive();
    }

    #endregion::攻撃

    #region::Utility

    //スクリーン座標を受け取る
    IntVect2D GetArrayFromRay(Vector2 touchPosition)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(touchPosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        //Tileのlayer番号は8
        var layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.tag == "Tile")
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                return hit.collider.GetComponent<TileBase>().positionArray;

            }
        }
        return null;
    }

    //タイル上のキャラを取得
    Character GetCharacterOnTile(IntVect2D toPos)
    {
        return GameObject.FindGameObjectsWithTag("BattleCharacter").
            Select(t => t.GetComponent<Character>()).
            Where(t => toPos.IsEqual(t.positionArray)).
            FirstOrDefault();
    }

    //タイル上のキャラが自身にとっての敵キャラなら取得
    Character GetOpponentCharacterOnTile(IntVect2D toPos)
    {

        var chara = GetCharacterOnTile(toPos);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;
    }
    Character GetOpponentCharacterFromTouch(Vector2 touchPosition)
    {

        var targetPosition = GetArrayFromRay(touchPosition);
        //タイル以外をタップ
        if (targetPosition == null) return null;
        //ターゲットの検索
        return GetOpponentCharacterOnTile(targetPosition);
    }

    #endregion::Utility
}
