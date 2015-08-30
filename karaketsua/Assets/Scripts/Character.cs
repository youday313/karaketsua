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

	GameObject actionUICanvas;

	enum CommandState{none,move,attack,skill};
	CommandState commandState=CommandState.none;
	void Start ()
	{
        positionArray = new Vect2D<int>((int)array.x, (int)array.y);
        animator = GetComponent<Animator>();
	}
    void Init(Vect2D<int> array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }
	
	void Update ()
	{
		
        
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
        Debug.Log("End");
        animator.SetFloat("Speed", 0f);
    }

	//キャラクターを行動選択状態にする
	public void OnSelect(){
		actionUICanvas.SetActive (true);
		isSelect = true;
	}
}