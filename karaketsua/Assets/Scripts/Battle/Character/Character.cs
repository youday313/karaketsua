//Character
//作成日
//<summary>
//キャラクターデータ
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public enum CommandState { None, Moved, TargetSelect, Attack, Skill, Wait, End };

public enum CharacterState { Wait, Attack, Skill, End }
public enum AttackDistance { Near, Middle, Far }

[System.Serializable]
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(CharacterAttacker))]

public class CharacterParameter
{
    public string charaName;
    //体力
    public int HP;
    public List<AttackParameter> attackParameter;
    //行動力
    public int activeSpeed;
    //攻撃力
    public int power;
    //防御力
    public int deffence;
    //精神力
    public int skillPoint;
    //知力
    public int intellect;



}
//一つの攻撃のパラメーター
[System.Serializable]
public class AttackParameter
{
    //攻撃範囲
    public List<IntVect2D> attackRange;

    //攻撃種類
    public AttackDistance attackDistance;

    //範囲攻撃
    //範囲内の全ての敵にダメージ
    public bool isMultiAttack;

    //技の威力
    public int power;


}

public class Character : MonoBehaviour
{


    //[System.NonSerialized]
    //public int movableCount=1;//移動可能距離
    //Animator animator;

    //リファクタリング後
    [System.NonSerialized]
    public CharacterState characterState = CharacterState.Wait;
    //現在のキャラクター位置配列
    [System.NonSerialized]
    public IntVect2D positionArray = new IntVect2D(0, 0);
    [System.NonSerialized]
    public bool isNowSelect = false;

    public CharacterParameter characterParameter;
    public GameObject deathEffect;
    [System.NonSerialized]
    public bool isEnemy = false;
    public GameObject activeCircle;

    ActiveTime activeTime;

    //移動用
    CharacterMover mover;
    //攻撃用
    //CharacterAttacker attacker;
    //スキル
    //Skill skill;

    CharacterMoveAttack skill;
    CharacterSingleAttack singleAttack;
    Animator animator;
    CharacterStateUI StateUI;
    CharacterDetailStateUI detailStateUI;

    CameraMove cameraMove;

    #region::初期化

    void Start()
    {
        mover = GetComponent<CharacterMover>();
        //attacker = GetComponent<CharacterAttacker>();
        animator = GetComponent<Animator>();
        skill = GetComponent<CharacterMoveAttack>();
        singleAttack = GetComponent<CharacterSingleAttack>();
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();

        activeTime = ActiveTimeCreater.Instance.CreateActiveTime(this);
        SetActiveTimeEventHandler();
        SetPositionOnTile();
        activeCircle.SetActive(false);
        DisableActionMode();
        CreateCharacterUI();
        //Init();
    }


    void SetPositionOnTile()
    {
        var tilePosition = BattleStage.Instance.GetTile(positionArray).transform.position;
        CSTransform.SetX(transform, tilePosition.x);
        CSTransform.SetZ(transform, tilePosition.z);
    }

    public void Init(IntVect2D array, bool isEne)
    {
        isEnemy = isEne;
        if (isEnemy == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        positionArray.x = array.x;
        positionArray.y = array.y;

    }
    void DisableActionMode()
    {
        isNowSelect = false;
        mover.ResetMoveParamEndAction();
        //attacker.IsEnable = false;
        singleAttack.IsEnable = false;
        skill.IsEnable = false;
        activeCircle.SetActive(false);
    }

    void SetActiveTimeEventHandler()
    {
        activeTime.OnStopActiveTimeE += OnActive;
    }

    void RemoveActiveTimeEventHandler()
    {
        activeTime.OnStopActiveTimeE -= OnActive;
    }
    void CreateCharacterUI()
    {
        if (isEnemy == true) return;
        StateUI = Instantiate(Resources.Load<CharacterStateUI>("CharacterStateUI")) as CharacterStateUI;
        StateUI.Init(this);
    }
    void UpdateCharacterStateUI()
    {
        if (isEnemy == true) return;
        StateUI.UpdateUI(this);
    }

    void OnEnable()
    {
        
        IT_Gesture.onShortTapE += OnTouchForDisplayState;
    }
    void OnDisable()
    {
        IT_Gesture.onShortTapE -= OnTouchForDisplayState;
    }
    #endregion::初期化

    void Update()
    {


    }

    public bool IsNowAction()
    {
        if (mover.isNowAction == true) return true;
        //if (attacker.isNowAction == true) return true;
        if (singleAttack.isNowAction == true) return true;
        if (skill.isNowAction == true) return true;
        return false;
    }

    public bool IsNowOnFinger()
    {
        if (mover.isNowCharge == true) return true;
        if (skill.isNowCharge == true) return true;
        return false;

    }

    //キャラクターを行動選択状態にする
    public void OnActive(ActiveTime wTime)
    {
        isNowSelect = true;
        SetInitialActionState();
        ActionSelect.Instance.OnActiveCharacter(this);
        activeCircle.SetActive(true);
        //タイル変更
        BattleStage.Instance.UpdateTileColors(this, TileState.Move);
    }

    void SetInitialActionState()
    {
        //attacker.IsEnable = false;
        singleAttack.IsEnable = false;
        skill.IsEnable = false;
        cameraMove.SetActiveCharacter(this);
        mover.IsEnable = true;


        //mover.Enable();

    }

    //ボタンからの行動決定
    public void SetAttackMode()
    {
        mover.IsEnable=false;

        //attacker.IsEnable = true;
        singleAttack.IsEnable = true;

        //BattleStage.Instance.UpdateTileColors(this, TileState.Attack);
    }
    public void ExecuteAttack()
    {
        //attacker.Attack();
        StartCoroutine(singleAttack.AttackWithTap());
    }
    public void ExcuteMoveAttack()
    {
        skill.ExcuteAttack();
    }
    public void SetSkillMode()
    {
        mover.IsEnable=false;
        skill.IsEnable = true;
        //BattleStage.Instance.UpdateTileColors(this, TileState.Skill);
    }

    public void SetWaitMode()
    {
        EndActiveCharacterAction();
    }
    public void SetAndo()
    {
        SetInitialActionState();
    }
    public void EndActiveCharacterAction()
    {
        BattleStage.Instance.ResetAllTileColor();
        activeTime.ResetValue();
        DisableActionMode();
        //PlayerOwner.Instance.OnEndActive();
        cameraMove.DisableActiveCharacter();
        ActionSelect.Instance.EndActiveAction();
    }
    public void Damage(int enemyPower)
    {
        var calcDamage = CalcDamage(enemyPower);
        characterParameter.HP -= calcDamage;
        CreateDamageText(calcDamage);
        //DamageAnimation();
        animator.SetTrigger("Damage");
        UpdateCharacterStateUI();

    }
    public void CheckDestroy()
    {
        if (characterParameter.HP <= 0)
        {
            animator.SetTrigger("Death");
            DeathMyself();
        }
    }
    void CreateDamageText(float damage)
    {
        //ダメージ表示
        var popupPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        var damageText = Instantiate(Resources.Load<Text>("DamageText"), Camera.main.WorldToScreenPoint(popupPosition), Quaternion.identity) as Text;
        damageText.text = damage.ToString();
        damageText.transform.SetParent(GameObject.FindGameObjectWithTag("EffectCanvas").transform);

    }
    //void DamageAnimation()
    //{
    //    if (characterParameter.HP == 0)
    //    {
    //        animator.SetTrigger("Death");
    //    }
    //    else
    //    {
    //        animator.SetTrigger("Damage");
    //    }
    //}
    //ここを変えるとダメージが変わる
    //計算式
    int CalcDamage(int power)
    {
        //相手攻撃力 - 自分防御力 がダメージ量
        //相手攻撃力<自分防御力 だったらダメージ0
        return Mathf.Max(0, power - (1 / 2 * characterParameter.deffence));
    }

    void DeathMyself()
    {
        //爆発エフェクト
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //リストから除く
        //WaitTimeManager.Instance.DestroyWaitTime(this.activeTime);
        RemoveActiveTimeEventHandler();
        activeTime.DeathCharacter();
        CharacterManager.Instance.DestroyCharacter(this);
        Destroy(gameObject);
    }


    //タッチでステータス表示
    void OnTouchForDisplayState(Vector2 touch)
    {
        if (detailStateUI == null)
        {
            if (CameraChange.Instance.nowCameraMode != CameraMode.Up) return;
            OnStartTouchForDisplayState(touch);
        }
        else
        {
            OnEndChargeForDisplayState();
        }
    }
    void OnStartTouchForDisplayState(Vector2 touch)
    {
        //ActiveTime停止中
        if (GameObject.FindGameObjectsWithTag("ActiveTime").Any(x => x.GetComponent<ActiveTime>().IsActive == true) == true) return;

        //ターゲットの検索
        var target = GetCharacterOnTile(touch);
        //ターゲットが存在しないマスをタップ
        if (target != this) return;

        //ターゲットが自分

        detailStateUI = Instantiate(Resources.Load<CharacterDetailStateUI>("CharacterDetailStateUI")) as CharacterDetailStateUI;
        detailStateUI.Init(touch, this.characterParameter);
    }
    void OnEndChargeForDisplayState()
    {
        if (detailStateUI == null) return;

        Destroy(detailStateUI.gameObject);
        detailStateUI = null;
    }


    #region::Utility
    //タイル上のキャラを取得
    //呼ばれる場所の統一化が必要
    public static Character GetCharacterOnTile(IntVect2D toPos)
    {

        var objects = GameObject.FindGameObjectsWithTag("BattleCharacter");
        var characters=new List<Character>();

        Debug.Log(objects.Length);
        foreach (var obj in objects)
        {
            if (obj == null)
            {

                Debug.Log("Error");
            }
            else
            {
                characters.Add(obj.GetComponent<Character>());
            }
        }
        Debug.Log(objects.Length);
        foreach (var ob in objects)
        {
            Debug.Log(ob.name);
        }

        foreach (var obj in characters)
        {
            if (IntVect2D.IsEqual(obj.positionArray, toPos) == true)
            {
                return obj;
            }
        }
        return null;
        //Linqが止まる原因か検証
        /*
    return GameObject.FindGameObjectsWithTag("BattleCharacter").
        Select(t => t.GetComponent<Character>()).
        Where(t => IntVect2D.IsEqual(toPos, t.positionArray)).
        FirstOrDefault();
         */
    }

    public static Character GetCharacterOnTile(Vector2 pos)
    {
        var targetPosition = TileBase.GetArrayFromRay(pos);
        //タイル以外をタップ
        if (targetPosition == null) return null;
        //ターゲットの検索
        var target = GetCharacterOnTile(targetPosition);

        //ターゲットが存在しないマスをタップ
        if (target == null) return null;
        return target;

    }

    #endregion::Utility







}