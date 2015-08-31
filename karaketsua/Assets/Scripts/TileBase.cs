//TileBase
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileBase : MonoBehaviour
{
	
    private Color defaultColor;  // 初期化カラー
    private Color selectColor;    // 選択時カラー
    //private Color movableColor;
    public Vect2D<int> positionArray;//配列番号
    
      protected Material material;

    public bool isMovableState=false;

    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        defaultColor = material.color;
        selectColor = Color.magenta;
        //movableColor = Color.blue;
    }
    public void Init(Vect2D<int> array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }
    //選択したキャラクターの下
    public void ChangeColor(bool isActive)
    {
        if (isActive)
        {
            material.color = selectColor;
        }
        else
        {
            material.color = defaultColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
