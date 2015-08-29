//Character
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour
{
	//public

	//private
    public Vect2D<int> positionArray;
    public bool isSelect;
    public int movableCount;//移動可能距離
    public Material material;
    Color defaultColor;
    Color selectColor;
    public Vector2 array;
    Animator animator;
	void Start ()
	{
        // このクラスが付属しているマテリアルを取得 
        material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        defaultColor = material.color;
        selectColor = Color.red;
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
        material.color = defaultColor;
        // StageBaseからbColorStateの値がtrueにされていれば色をかえる 
        if (isSelect)
        {
            material.color = selectColor;
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
        Debug.Log("End");
        animator.SetFloat("Speed", 0f);
    }
}