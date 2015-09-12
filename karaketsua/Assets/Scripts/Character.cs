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


	void Start ()
	{
        positionArray = new IntVect2D((int)array.x, (int)array.y);
        animator = GetComponent<Animator>();
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
	
	void Update ()
	{
		
        
	}

    //
    public void Move(IntVect2D toVect)
    {
        //既に移動していた
        if (isAlreadyMove == true) return;

        positionArray.x = Mathf.Clamp(positionArray.x + toVect.x, -BattleStage.stageSizeX, BattleStage.stageSizeX);
        positionArray.y = Mathf.Clamp(positionArray.y + toVect.y, -BattleStage.stageSizeY, BattleStage.stageSizeY);

        //移動先のタイル
        var toTile = GameObject.FindGameObjectsWithTag("Tile").Select(x => x.GetComponent<TileBase>())
            .Where(t => t.positionArray.x == positionArray.x && t.positionArray.y == positionArray.y).FirstOrDefault();
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
        ResetTileColor();
        //自分の乗っているタイルの色変更
        toTile.ChangeColor(TileState.Moved);
        PlayerOwner.Instance.commandState = CommandState.Moved;

    }

    public void SetTarget(Vector2 touchPosition )
    {

        IntVect2D targetPosition = GetArrayFromRay(touchPosition);
        //タイル以外をタップ
        if (IntVect2D.IsNull(targetPosition)) return;
        //ターゲットの検索
        var target = GameObject.FindGameObjectsWithTag("EnemyCharacter")
    .Select(t => t.GetComponent<Character>())
    .Where(t => IntVect2D.IsEqual(t.positionArray, targetPosition))
    .FirstOrDefault();
        //ターゲットが存在しないマスをタップ
        if (target == null) return;
        //攻撃範囲内

        if (Mathf.Abs(targetPosition.x - positionArray.x) + Mathf.Abs(targetPosition.y - positionArray.y) > 1) return;


        attackTarget = target;
        //ターゲットのタイル変更
        var toTile = GameObject.FindGameObjectsWithTag("Tile").Select(x => x.GetComponent<TileBase>())
.Where(t => t.positionArray.x == targetPosition.x && t.positionArray.y == targetPosition.y).FirstOrDefault();
        toTile.ChangeColor(TileState.Select);


        PlayerOwner.Instance.commandState = CommandState.Attack;

    }

    //ターゲットが選択されていた時タイルタップ
    public void Attack(Vector2 touchPosition)
    {
        var targetPosition = GetArrayFromRay(touchPosition);
        //タイル以外をタップ
        if (IntVect2D.IsNull(targetPosition)) return;

        //ターゲットの検索
        var target = GameObject.FindGameObjectsWithTag("EnemyCharacter")
    .Select(t => t.GetComponent<Character>())
    .Where(t => IntVect2D.IsEqual(t.positionArray, targetPosition))
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
            ChangeNeighborTile(TileState.Attack);
            SetTarget(touchPosition);
        }

        //攻撃
        animator.SetTrigger("Attack");
        target.Damage(1);
        PlayerOwner.Instance.commandState = CommandState.End;
    }

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

    //攻撃待機状態
    public void SetAttackMode()
    {
        ResetTileColor();
        //自分の乗っているタイルの色変更
        var toTile = GameObject.FindGameObjectsWithTag("Tile").Select(x => x.GetComponent<TileBase>())
        .Where(t => t.positionArray.x == positionArray.x && t.positionArray.y == positionArray.y).FirstOrDefault();
        toTile.ChangeColor(TileState.Moved);

        //攻撃範囲のタイル色を攻撃可能色にする
        //現在は上下左右のみ
        var tiles = GameObject.FindGameObjectsWithTag("Tile")
            .Select(t => t.GetComponent<TileBase>())
            .Where(t =>
               t.positionArray.x - 1 == positionArray.x && t.positionArray.y == positionArray.y
            || t.positionArray.x + 1 == positionArray.x && t.positionArray.y == positionArray.y
            || t.positionArray.x == positionArray.x && t.positionArray.y - 1 == positionArray.y
            || t.positionArray.x == positionArray.x && t.positionArray.y + 1 == positionArray.y)
            .Select(t => t.GetComponent<TileBase>());
        foreach (var t in tiles)
        {
            t.ChangeColor(TileState.Attack);
        }
    }

    public void Damage(float power)
    {
        hitPoint -= power;
        if (hitPoint <= 0)
        {
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }


    void CompleteMove()
    {
        animator.SetFloat("Speed", 0f);
    }

	//キャラクターを行動選択状態にする
	public void OnSelect(){
		isSelect = true;
        isAlreadyMove = false;
        //選択したキャラを登録
        PlayerOwner.Instance.OnActiveCharacter(this);
        //足元のタイルの色変更
        var tile=GameObject.FindGameObjectsWithTag("Tile").Where(t => t.GetComponent<TileBase>().positionArray.x == positionArray.x && t.GetComponent<TileBase>().positionArray.y == positionArray.y)
            .Select(t => t.GetComponent<TileBase>()).FirstOrDefault();
        tile.ChangeColor(TileState.Select);
            
        //上下左右のタイル色を移動可能色にする
        ChangeNeighborTile(TileState.Active);

	}
    //上下左右のタイル色変更
    public void ChangeNeighborTile(TileState toState)
    {
        var tiles = GameObject.FindGameObjectsWithTag("Tile")
    .Select(t => t.GetComponent<TileBase>())
    .Where(t =>
       t.positionArray.x - 1 == positionArray.x && t.positionArray.y == positionArray.y
    || t.positionArray.x + 1 == positionArray.x && t.positionArray.y == positionArray.y
    || t.positionArray.x == positionArray.x && t.positionArray.y - 1 == positionArray.y
    || t.positionArray.x == positionArray.x && t.positionArray.y + 1 == positionArray.y)
    .Select(t => t.GetComponent<TileBase>());
        foreach (var t in tiles)
        {
            t.ChangeColor(toState);
        }

    }

    public void SetSkillMode()
    {
        ResetTileColor();
        //ルート2以下の距離
        var tiles = GameObject.FindGameObjectsWithTag("Tile")
            .Select(t => t.GetComponent<TileBase>())
            .Where(t => Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(positionArray.x, positionArray.y)) < 2f)
            .Select(t => t.GetComponent<TileBase>());
        //色変更
        foreach (var t in tiles)
        {
            t.ChangeColor(TileState.Skill);
        }
        //足元のタイルの色変更
        var tile = GameObject.FindGameObjectsWithTag("Tile").Where(t => t.GetComponent<TileBase>().positionArray.x == positionArray.x && t.GetComponent<TileBase>().positionArray.y == positionArray.y)
            .Select(t => t.GetComponent<TileBase>()).FirstOrDefault();
        tile.ChangeColor(TileState.Select);

        swipedPosision = new List<IntVect2D>();
    }

    List<IntVect2D> swipedPosision = new List<IntVect2D>();
    
    public void SkillSwipe(SwipeInfo swipeInfo)
    {
                     var targetPosition=GetArrayFromRay(swipeInfo.startPoint);
            if(IntVect2D.IsNull(targetPosition)==true)return;

        //一番初めのスワイプ
        if (swipedPosision.Count==0)
        {
            Debug.Log("First");
            //ルート2以下の距離
             if (Vector2.Distance(new Vector2(targetPosition.x, targetPosition.y), new Vector2(positionArray.x, positionArray.y)) >= 2f) return;
             //スキル開始
             swipedPosision.Add(targetPosition);
             var tile = GameObject.FindGameObjectsWithTag("Tile")
     .Select(t => t.GetComponent<TileBase>())
     .Where(t => IntVect2D.IsEqual(targetPosition, t.positionArray))
     .FirstOrDefault();
             tile.ChangeColor(TileState.Select);

        }
        else
        {
            var lastPosition = swipedPosision.Last();
            //現在位置
            if (IntVect2D.IsEqual(targetPosition, lastPosition)==true) return;
            //隣
            if(IntVect2D.IsNeighbor(targetPosition,lastPosition)==false)return;
            //キャラクター以外
            if (IntVect2D.IsEqual(targetPosition,positionArray)) return;
                //キャラクターから2未満
            if (Vector2.Distance(new Vector2(targetPosition.x, targetPosition.y), new Vector2(positionArray.x, positionArray.y)) >= 2f) return;
            //まだ通過していない
            foreach (var pos in swipedPosision)
            {
                if(IntVect2D.IsEqual(targetPosition, pos)==true)return;
            }

            //新しい位置
            swipedPosision.Add(targetPosition);
            //ルート2以下の距離
            var tile = GameObject.FindGameObjectsWithTag("Tile")
                .Select(t => t.GetComponent<TileBase>())
                .Where(t => IntVect2D.IsEqual(targetPosition, t.positionArray))
                .FirstOrDefault();
            tile.ChangeColor(TileState.Select);

            if (swipedPosision.Count == 8)
            {
                //ターゲットの検索
                var targets = GameObject.FindGameObjectsWithTag("EnemyCharacter")
                    .Select(t => t.GetComponent<Character>())
                    .Where(t => Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(positionArray.x, positionArray.y))<2f)
                    .Select(t=>t.GetComponent<Character>());
                if (targets.Count() == 0) return;        
                foreach (var tar in targets)
                {
                    tar.Damage(1);
                }
                //攻撃
                animator.SetTrigger("Attack");
            }

        }
    }


    //床のタイルの色をクリア
    public void ResetTileColor()
    {
        foreach(var tile in GameObject.FindGameObjectsWithTag("Tile").Select(x=>x.GetComponent<TileBase>())){
            tile.ChangeColor(TileState.Default);
        }
    }



    void ResetActive()
    {
        waitTime.ResetValue();

    }

}