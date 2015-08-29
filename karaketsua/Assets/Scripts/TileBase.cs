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

    private Color default_color;  // 初期化カラー
    private Color select_color;    // 選択時カラー
    private Color movableColor;
    public Vect2D<int> positionArray;//配列番号
    
    protected Material _material;

    public bool bColorState=false;
    public bool isMovableState=false;

    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        _material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = _material.color;
        select_color = Color.magenta;
        movableColor = Color.blue;
    }
    public void Init(Vect2D<int> array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }
    

    // Update is called once per frame
    void Update()
    {
        _material.color = default_color;
        // StageBaseからbColorStateの値がtrueにされていれば色をかえる
        if (isMovableState)
        {
            _material.color = movableColor;
        }
        if (bColorState)
        {
            bColorState = false;
            _material.color = select_color;
        }
    }
}
