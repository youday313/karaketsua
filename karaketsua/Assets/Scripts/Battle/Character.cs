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

public enum CommandState { None, Moved, TargetSelect, Attack, Skill, Wait, End };

public enum CharacterState { Wait,Attack,Skill,End}

[System.Serializable]
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(CharacterAttacker))]

public class CharacterParameter
{
    public string charaName;
    //体力
    public int HP;
    //攻撃範囲
    public int attackRange=1;
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

public class Character : MonoBehaviour
{


    //[System.NonSerialized]
    //public int movableCount=1;//移動可能距離
    //Animator animator;

    //リファクタリング後
    [System.NonSerialized]
    public CharacterState characterState = CharacterState.Wait;
    //現在のキャラクター位置配列
    public IntVect2D positionArray = new IntVect2D(0, 0);
    [System.NonSerialized]
    public bool isNowSelect = false;
    
    public CharacterParameter characterParameter;
    public GameObject deathEffect;
    [System.NonSerialized]
    public bool isEnemy = false;
    //乗っているタイル
    TileBase onTile;
    public GameObject activeCircle;

    ActiveTime activeTime;

    //移動用
    CharacterMover mover;
    //攻撃用
    CharacterAttacker attacker;
    //スキル
    //Skill skill;

    CharacterSkill skill; 
    Animator animator;




    #region::初期化

    void Start ()
	{
        mover = GetComponent<CharacterMover>();
        attacker = GetComponent<CharacterAttacker>();
        animator = GetComponent<Animator>();
        skill = GetComponent<CharacterSkill>();
        activeTime = ActiveTimeCreater.Instance.CreateActiveTime(this);
        SetActiveTimeEventHandler();
        SetPositionOnTile();
        activeCircle.SetActive(false);
        DisableActionMode();
        //Init();
    }


    void SetPositionOnTile()
    {
        var tilePosition = BattleStage.Instance.GetTile(positionArray).transform.position;
        CSTransform.SetX(transform,tilePosition.x);
        CSTransform.SetZ(transform,tilePosition.z);
    }

    public void Init(IntVect2D array,bool isEne)
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
        mover.Disable();
        attacker.IsEnable = false;
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
    #endregion::初期化

    void Update ()
	{
        
        
	}

    public bool IsNowAction()
    {
        if (mover.isNowAction == true) return true;
        if (attacker.isNowAction == true) return true;
        if (skill.isNowAction == true) return true;
        return false;
    }

    //キャラクターを行動選択状態にする
    public void OnActive(ActiveTime wTime)
    {
        isNowSelect = true;
        SetInitialActionState(true);
        ActionSelect.Instance.SetActiveAction(this);
        activeCircle.SetActive(true);
        //タイル変更
        BattleStage.Instance.UpdateTileColors(this, TileState.Move);

        CameraMove.Instance.MoveToBack(this);
    }
    void SetInitialActionState(bool isAllReset)
    {
        attacker.IsEnable = false;
        skill.IsEnable = false;
        CameraMove.Instance.MoveToBack(this);
        if (isAllReset == true)
        {
            mover.OnActiveCharacter();
        }
        else
        {
            mover.Enable();
        }
        
        //mover.Enable();
        
    }
    
    //ボタンからの行動決定
    public void SetAttackMode()
    {
        mover.Disable();

        attacker.IsEnable = true;
        BattleStage.Instance.UpdateTileColors(this, TileState.Attack);
    }
    public void SetSkillMode()
    {
        mover.Disable();
        skill.IsEnable = true;
        BattleStage.Instance.UpdateTileColors(this, TileState.Skill);
    }

    public void SetWaitMode()
    {
        ResetActive();
    }
    public void SetAndo()
    {
        SetInitialActionState(false);
    }
    public void ResetActive()
    {
        BattleStage.Instance.ResetTileColor();
        activeTime.ResetValue();
        DisableActionMode();
        //PlayerOwner.Instance.OnEndActive();
        CameraMove.Instance.MoveToLean();
    }
    public void Damage(int enemyPower)
    {
        characterParameter.HP -= CalcDamage(enemyPower);
        DamageAnimation();
        if (characterParameter.HP <= 0)
        {
            DeathMyself();
        }
        
    }
    void DamageAnimation()
    {
        if (characterParameter.HP == 0)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
    //計算式
    int CalcDamage(int power)
    {
        return power;
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

    #region::Utility
    //クリックしたタイル位置を取得
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

    #endregion:Utility






    #region::スキル
    /*
    List<IntVect2D> swipedPosision = new List<IntVect2D>();
    public void SetSkillMode()
    {

        //ルート2以下の距離
        //色変更
        //battleStage.ChangeTilesColorFromDistance(positionArray, TileState.Skill, Mathf.Sqrt(2), reset:true);
        for (var i = -BattleStage.stageSizeX; i <= BattleStage.stageSizeX; i++)
        {
            var pos = new IntVect2D(positionArray);
            pos.x = i;
            BattleStage.Instance.UpdateTileColors(new IntVect2D(pos), TileState.Skill);
        }
        //for (var i = -BattleStage.stageSizeY; i <= BattleStage.stageSizeY; i++)
        //{
        //    var pos = new IntVect2D(positionArray);
        //    pos.y = i;
        //    battleStage.ChangeColor(pos, TileState.Skill);
        //}


        //足元のタイルの色変更
        //BattleStage.Instance.ChangeColor(positionArray, TileState.Select);

        swipedPosision = new List<IntVect2D>();
    }
    bool skillFirstStep=true;
    public void SkillSwipe(SwipeInfo swipeInfo)
    {
        var targetPosition = GetArrayFromRay(swipeInfo.startPoint);
        if (IntVect2D.IsNull(targetPosition) == true) return;

        if (skillFirstStep == true)
        {
            //一番初めのスワイプ
            if (swipedPosision.Count == 0)
            {
                //一番左から
                var leftPos = new IntVect2D(-BattleStage.stageSizeX, positionArray.y);
                if (!targetPosition.IsEqual(leftPos)) return;
                //スキル開始
                swipedPosision.Add(targetPosition);

                BattleStage.Instance.ChangeColor(targetPosition, TileState.Skilled);
            }
            else
            {
                var lastPosition = swipedPosision.Last();
                //現在位置
                if (IntVect2D.IsEqual(targetPosition, lastPosition) == true) return;
                //右隣
                if (IntVect2D.IsNeighbor(targetPosition, lastPosition) == false) return;
                if (lastPosition.y != targetPosition.y || lastPosition.x > targetPosition.x) return;
                //まだ通過していない
                foreach (var pos in swipedPosision)
                {
                    if (IntVect2D.IsEqual(targetPosition, pos) == true) return;
                }
                //新しい位置
                swipedPosision.Add(targetPosition);
                //ルート2以下の距離
                BattleStage.Instance.ChangeColor(targetPosition, TileState.Skilled);
                //全て塗った
                if (swipedPosision.Count == BattleStage.stageSizeX * 2 + 1)
                {
                    skillFirstStep = false;
                    //次はYを塗る
                    for (var i = -BattleStage.stageSizeY; i <= BattleStage.stageSizeY; i++)
                    {
                        var pos = new IntVect2D(positionArray);
                        pos.y = i;
                        BattleStage.Instance.ChangeColor(pos, TileState.Skill);
                    }
                    swipedPosision = new List<IntVect2D>();
                    ////ターゲットの検索
                    //var targets = GameObject.FindGameObjectsWithTag("EnemyCharacter")
                    //    .Select(t => t.GetComponent<Character>())
                    //    .Where(t => Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(positionArray.x, positionArray.y)) < 2f)
                    //    .Select(t => t.GetComponent<Character>());
                    //if (targets.Count() == 0) return;
                    //foreach (var tar in targets)
                    //{
                    //    tar.Damage(1);
                    //}
                    ////攻撃
                    //animator.SetTrigger("Attack");
                    //ResetActive();
                }

            }
        }
        else
        {
            //一番初めのスワイプ
            if (swipedPosision.Count == 0)
            {
                //一番上から
                var upPos = new IntVect2D(positionArray.x,BattleStage.stageSizeY);
                if (!targetPosition.IsEqual(upPos)) return;
                //スキル開始
                swipedPosision.Add(targetPosition);

                BattleStage.Instance.ChangeColor(targetPosition, TileState.Skilled);
            }
            else
            {
                var lastPosition = swipedPosision.Last();
                //現在位置
                if (IntVect2D.IsEqual(targetPosition, lastPosition) == true) return;
                //下以外
                if (IntVect2D.IsNeighbor(targetPosition, lastPosition) == false) return;
                if (lastPosition.y < targetPosition.y || lastPosition.x != targetPosition.x) return;
                //まだ通過していない
                foreach (var pos in swipedPosision)
                {
                    if (IntVect2D.IsEqual(targetPosition, pos) == true) return;
                }
                //新しい位置
                swipedPosision.Add(targetPosition);
                //ルート2以下の距離
                BattleStage.Instance.ChangeColor(targetPosition, TileState.Skilled);
                if (swipedPosision.Count == BattleStage.stageSizeY * 2 + 1)
                {
                    //skillFirstStep = false;
                    ////ターゲットの検索
                    var targets = GameObject.FindGameObjectsWithTag("EnemyCharacter")
                        .Select(t => t.GetComponent<Character>())
                        .Where(t =>  t.positionArray.x==positionArray.x||t.positionArray.y==positionArray.y)
                        .Select(t => t.GetComponent<Character>());
                    if (targets.Count() == 0) return;
                    foreach (var tar in targets)
                    {
                        //tar.Damage(1);
                    }
                    //攻撃
                    animator.SetTrigger("Attack");
                    ResetActive();
                }

            }
        }
    }


    /*public void SkillSwipe(SwipeInfo swipeInfo)
    {
        var targetPosition = GetArrayFromRay(swipeInfo.startPoint);
        if (IntVect2D.IsNull(targetPosition) == true) return;

        //一番初めのスワイプ
        if (swipedPosision.Count == 0)
        {
            //ルート2以下の距離
            if (Vector2.Distance(new Vector2(targetPosition.x, targetPosition.y), new Vector2(positionArray.x, positionArray.y)) > Mathf.Sqrt(2)) return;
            //スキル開始
            swipedPosision.Add(targetPosition);

            battleStage.ChangeColor(targetPosition, TileState.Select);
        }
        else
        {
            var lastPosition = swipedPosision.Last();
            //現在位置
            if (IntVect2D.IsEqual(targetPosition, lastPosition) == true) return;
            //隣
            if (IntVect2D.IsNeighbor(targetPosition, lastPosition) == false) return;
            //キャラクター以外
            if (IntVect2D.IsEqual(targetPosition, positionArray)) return;
            //キャラクターから2未満
            if (Vector2.Distance(new Vector2(targetPosition.x, targetPosition.y), new Vector2(positionArray.x, positionArray.y)) >= 2f) return;
            //まだ通過していない
            foreach (var pos in swipedPosision)
            {
                if (IntVect2D.IsEqual(targetPosition, pos) == true) return;
            }

            //新しい位置
            swipedPosision.Add(targetPosition);
            //ルート2以下の距離
            battleStage.ChangeColor(targetPosition, TileState.Select);

            if (swipedPosision.Count == 8)
            {
                //ターゲットの検索
                var targets = GameObject.FindGameObjectsWithTag("EnemyCharacter")
                    .Select(t => t.GetComponent<Character>())
                    .Where(t => Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(positionArray.x, positionArray.y)) < 2f)
                    .Select(t => t.GetComponent<Character>());
                if (targets.Count() == 0) return;
                foreach (var tar in targets)
                {
                    tar.Damage(1);
                }
                //攻撃
                animator.SetTrigger("Attack");
                ResetActive();
            }

        }
    }
    */
    #endregion::スキル
    




}