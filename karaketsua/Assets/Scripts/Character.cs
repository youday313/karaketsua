//Character
//作成日
//<summary>
//キャラクターデータ
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour
{
	//public

	//private
    //現在のキャラクター位置配列
	public Vect2D<int> positionArray;

    bool isSelect=false;
	[System.NonSerialized]
	public int movableCount=1;//移動可能距離
    public Vector2 array;
    Animator animator;

    //パラメーター
    public float hitPoint=1;
    public GameObject destroyEffect;
    public WaitTime waitTime;

	void Start ()
	{
        positionArray = new Vect2D<int>((int)array.x, (int)array.y);
        animator = GetComponent<Animator>();
        Init();
	}
    void Init()
    {
        positionArray.x = (int)array.x;
        positionArray.y = (int)array.y;
    }
	
    void Init(Vect2D<int> array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }
	
	void Update ()
	{
		
        
	}

    //
    public void Move(Vect2D<int> toVect)
    {


        positionArray.x = Mathf.Clamp(positionArray.x + toVect.x, -BattleStage.stageSizeX, BattleStage.stageSizeX);
        positionArray.y = Mathf.Clamp(positionArray.y + toVect.y, -BattleStage.stageSizeY, BattleStage.stageSizeY);

        Hashtable table = new Hashtable();
        var toTile = GameObject.FindGameObjectsWithTag("Tile").Select(x => x.GetComponent<TileBase>())
            .Where(t => t.positionArray.x == positionArray.x && t.positionArray.y == positionArray.y).FirstOrDefault(); ;

        table.Add("x",toTile.transform.position.x);
        table.Add("z", toTile.transform.position.z);
        table.Add("time", 1.0f);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        animator.SetFloat("Speed", 1f);
        iTween.MoveTo(gameObject, table);


    }

    public void Attack(Vect2D<int> toVect)
    {
        //攻撃先
        var attackToEnmeyPosition = new Vect2D<int>(positionArray.x + toVect.x, positionArray.y + toVect.y);

        var target = GameObject.FindGameObjectsWithTag("EnemyCharacter").Select(e => e.GetComponent<Character>())
            .Where(e => e.positionArray.x == attackToEnmeyPosition.x && e.positionArray.y == attackToEnmeyPosition.y).FirstOrDefault();
        if (target != null)
        {
            animator.SetTrigger("Attack");
            target.Damage(1);
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

    public void Move(Vector3 toVect)
    {
        Hashtable table = new Hashtable();
        table.Add("x", toVect.x);
        table.Add("z", toVect.z);
        table.Add("time", 1.0f);
        table.Add("easetype", iTween.EaseType.linear);
        table.Add("oncomplete", "CompleteMove");	// トゥイーン終了時にCompleteHandler()を呼ぶ
        animator.SetFloat("Speed", 1f);
        iTween.MoveTo(gameObject, table);
    }

    void CompleteMove()
    {
        animator.SetFloat("Speed", 0f);
        ResetActive();
    }

	//キャラクターを行動選択状態にする
	public void OnSelect(){
		isSelect = true;
        //選択したキャラを登録
        PlayerOwner.Instance.OnActiveCharacter(this);
        //足元のタイルの色変更

        var tile=GameObject.FindGameObjectsWithTag("Tile").Where(t => t.GetComponent<TileBase>().positionArray.x == positionArray.x && t.GetComponent<TileBase>().positionArray.y == positionArray.y)
            .Select(t => t.GetComponent<TileBase>()).FirstOrDefault();
        tile.ChangeColor(isActive:true);
            


	}

    void ResetActive()
    {
        waitTime.ResetValue();

    }
}