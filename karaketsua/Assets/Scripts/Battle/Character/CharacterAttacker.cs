using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
public class CharacterAttacker : MonoBehaviour {

    protected Character character;
        [System.NonSerialized]
    public bool isNowAction = false;
        [System.NonSerialized]
    public bool isSetTarget = false;
    protected bool isEnable = false;
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

    protected List<Character> attackTarget=new List<Character>();
    protected CameraMove cameraMove;
    //選択した攻撃方法
    [System.NonSerialized]
    public AttackParameter selectAttackParameter=null;

	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
	}

    protected void OnActiveCharacter()
    {
        isSetTarget = false;
        isNowAction = false;
        attackTarget = new List<Character>();
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
        //攻撃
        foreach (var target in attackTarget)
        {
            target.Damage(character.characterParameter.power);
        }
        StartAttackAnimation();
        //攻撃時にUI非表示
        ActionSelect.Instance.EndActiveAction();

    }

    //タップで攻撃
    public float changeTimeTapMode = 1f;
    float tapJustTime = 5f;
    public GameObject attackMakerPrefab;
    bool isTapDetect = false;
    float startTime;
    float tapLeftTime;
    public IEnumerator AttackWithTap()
    {
        if (attackTarget.Count == 0) yield return null;

        //カメラ移動
        cameraMove.MoveToTapAttack(this, attackTarget[0].transform.position, changeTimeTapMode);
        yield return new WaitForSeconds(changeTimeTapMode);
        
        //攻撃アニメーション
        animator.SetTrigger("TapAttack");
        isNowAction = true;
        //Invoke("OnCompleteAnimation", attackMotionTime);

        //マーカー表示
        var popupPosition=new Vector3(attackTarget[0].transform.position.x, attackTarget[0].transform.position.y+1f, attackTarget[0].transform.position.z);
        var attackMaker = Instantiate(attackMakerPrefab, Camera.main.WorldToScreenPoint(popupPosition),Quaternion.identity) as GameObject;

        attackMaker.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);
        
        Destroy(attackMaker,tapJustTime);

        //とりあえず2秒待ってからマーカー縮小
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(attackMaker, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", tapJustTime-2));

        //タップ判定
        startTime = Time.time;
        //タップできなかったら最大時間
        tapLeftTime = 0;
        IT_Gesture.onShortTapE += OnTapForAttack;
        yield return new WaitForSeconds(tapJustTime-2);
        IT_Gesture.onShortTapE -= OnTapForAttack;
        isTapDetect = false;

        //攻撃
        foreach (var target in attackTarget)
        {
            //target.Damage(character.characterParameter.power);
            //攻撃力に関わらず秒数
            target.Damage((int)(tapLeftTime * 1000));

        }
        attackTarget = null;
        
        Invoke("OnCompleteAnimation",attackMotionTime);
        //攻撃時にUI非表示
        ActionSelect.Instance.EndActiveAction();
        yield return null;

    }
    //タイミングを合わせたタップ
    void OnTapForAttack(Vector2 pos)
    {
        if (isTapDetect == true) return;
        isTapDetect = true;
        //残り時間
        var nowTime = Time.time - startTime;
        tapLeftTime = Mathf.Clamp(nowTime,0,tapJustTime);


    }



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
    protected Character GetOpponentCharacterOnTile(IntVect2D toPos)
    {

        var chara = Character.GetCharacterOnTile(toPos);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;
    }
    protected Character GetOpponentCharacterFromTouch(Vector2 touchPosition)
    {
        var chara = Character.GetCharacterOnTile(touchPosition);
        if (chara == null) return null;
        if (chara.isEnemy != this.character.isEnemy) return chara;
        return null;

    }
}
