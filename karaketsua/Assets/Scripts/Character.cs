﻿//Character
//作成日
//<summary>
//キャラクターデータ
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Character : MonoBehaviour
{
	//public

	//private
    //現在のキャラクター位置配列
	public IntVect2D positionArray;

    bool isSelect=false;
	[System.NonSerialized]
	public int movableCount=1;//移動可能距離
    public Vector2 array;
    Animator animator;

    //パラメーター
    public float hitPoint=1;
    public GameObject destroyEffect;
    public WaitTime waitTime;
    public bool isAlreadyMove = false;
    Character attackTarget;

    BattleStage battleStage;

    #region::初期化

    void Start ()
	{
        positionArray = new IntVect2D((int)array.x, (int)array.y);
        animator = GetComponent<Animator>();
        battleStage = BattleStage.Instance;
        Init();
	}
    void Init()
    {
        positionArray.x = (int)array.x;
        positionArray.y = (int)array.y;

    }

    void Init(IntVect2D array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }

    #endregion::初期化

    void Update ()
	{
		
        
	}


    #region::移動
    //
    public void Move(IntVect2D toVect)
    {
        
        //既に移動していた
        if (isAlreadyMove == true) return;

        positionArray.x = Mathf.Clamp(positionArray.x + toVect.x, -BattleStage.stageSizeX, BattleStage.stageSizeX);
        positionArray.y = Mathf.Clamp(positionArray.y + toVect.y, -BattleStage.stageSizeY, BattleStage.stageSizeY);

        //移動先のタイル
        var toTile = battleStage.GetTile(positionArray);
        //移動失敗
        if (toTile == null) return;

        Hashtable table = new Hashtable();
        table.Add("x",toTile.transform.position.x);
        table.Add("z", toTile.transform.position.z);
        table.Add("time", 1.0f);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        animator.SetFloat("Speed", 1f);
        iTween.MoveTo(gameObject, table);
        isAlreadyMove = true;


        //自分の乗っているタイルの色変更
        battleStage.ChangeColor(positionArray,TileState.Moved,reset:true);
        PlayerOwner.Instance.commandState = CommandState.Moved;

    }
    void CompleteMove()
    {
        animator.SetFloat("Speed", 0f);
    }
    #endregion::移動
    #region::選択
    //キャラクターを行動選択状態にする
    public void OnSelect()
    {
        isSelect = true;
        isAlreadyMove = false;
        //選択したキャラを登録
        PlayerOwner.Instance.OnActiveCharacter(this);
        //足元のタイルの色変更
        battleStage.ChangeColor(positionArray, TileState.Select);

        //上下左右のタイル色を移動可能色にする
        battleStage.ChangeNeighborTilesColor(positionArray, TileState.Active);

    }
    //ターゲットの決定
    public void SetTarget(Vector2 touchPosition)
    {

        IntVect2D targetPosition = GetArrayFromRay(touchPosition);
        //タイル以外をタップ
        if (IntVect2D.IsNull(targetPosition)) return;
        //ターゲットの検索
        var target = GameObject.FindGameObjectsWithTag("EnemyCharacter")
    .Select(t => t.GetComponent<Character>())
    .Where(t => targetPosition.IsEqual(t.positionArray))
    .FirstOrDefault();
        //ターゲットが存在しないマスをタップ
        if (target == null) return;
        //攻撃範囲内

        if (Mathf.Abs(targetPosition.x - positionArray.x) + Mathf.Abs(targetPosition.y - positionArray.y) > 1) return;


        attackTarget = target;
        //ターゲットのタイル変更
        battleStage.ChangeColor(positionArray, TileState.Select);



        PlayerOwner.Instance.commandState = CommandState.Attack;

    }
    //攻撃待機状態
    public void SetAttackMode()
    {


        //自分の乗っているタイルの色変更
        battleStage.ChangeColor(positionArray, TileState.Moved, reset: true);

        //攻撃範囲のタイル色を攻撃可能色にする
        //現在は上下左右のみ
        battleStage.ChangeNeighborTilesColor(positionArray, TileState.Attack);

    }
    #endregion::選択

    #region::攻撃
    //ターゲットが選択されていた時タイルタップ
    public void Attack(Vector2 touchPosition)
    {
        var targetPosition = GetArrayFromRay(touchPosition);
        //タイル以外をタップ
        if (IntVect2D.IsNull(targetPosition)) return;

        //ターゲットの検索
        var target = GameObject.FindGameObjectsWithTag("EnemyCharacter")
    .Select(t => t.GetComponent<Character>())
    .Where(t => targetPosition.IsEqual(t.positionArray))
    .FirstOrDefault();

        //ターゲットが存在しないマスをタップ
        if (target == null)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.TargetSelect);
            //ターゲットのタイル変更
            return;

        }
        //ターゲット以外のキャラをタップ
        //ターゲットの切り替え
        else if (target != attackTarget)
        {
            battleStage.ChangeNeighborTilesColor(positionArray, TileState.Attack);
            SetTarget(touchPosition);
        }

        //攻撃
        animator.SetTrigger("Attack");
        target.Damage(1);
        ResetActive();
    }


    public void Damage(float power)
    {
        hitPoint -= power;
        if (hitPoint <= 0)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion::攻撃




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
        return new IntVect2D(IntVect2D.nullNumber,IntVect2D.nullNumber);
    }

    #endregion:Utility






    #region::スキル

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
            battleStage.ChangeColor(new IntVect2D(pos), TileState.Skill);
        }
        //for (var i = -BattleStage.stageSizeY; i <= BattleStage.stageSizeY; i++)
        //{
        //    var pos = new IntVect2D(positionArray);
        //    pos.y = i;
        //    battleStage.ChangeColor(pos, TileState.Skill);
        //}


        //足元のタイルの色変更
        battleStage.ChangeColor(positionArray, TileState.Select);

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

                battleStage.ChangeColor(targetPosition, TileState.Select);
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
                battleStage.ChangeColor(targetPosition, TileState.Select);
                if (swipedPosision.Count == BattleStage.stageSizeX * 2 + 1)
                {
                    skillFirstStep = false;
                    for (var i = -BattleStage.stageSizeY; i <= BattleStage.stageSizeY; i++)
                    {
                        var pos = new IntVect2D(positionArray);
                        pos.y = i;
                        battleStage.ChangeColor(pos, TileState.Skill);
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

                battleStage.ChangeColor(targetPosition, TileState.Select);
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
                battleStage.ChangeColor(targetPosition, TileState.Select);
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
                        tar.Damage(1);
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




    public void Wait()
    {
        ResetActive();
    }

    void ResetActive()
    {
        battleStage.ResetTileColor();
        waitTime.ResetValue();
        PlayerOwner.Instance.OnEndActive();
    }

}