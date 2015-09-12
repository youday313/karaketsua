//TileBase
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public enum TileState { Default=0,Active,Moved,Select,Attack,Skill};

public class TileBase : MonoBehaviour
{

    //[SerializeField]
    Color[] tileColors=new Color[Enum.GetNames(typeof(TileState)).Count()];
    public IntVect2D positionArray = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);//配列番号
    
      protected Material material;

    public bool isMovableState=false;

    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        tileColors[(int)TileState.Default] = material.color;
        tileColors[(int)TileState.Active] = Color.blue;
        tileColors[(int)TileState.Moved] = Color.green;
        tileColors[(int)TileState.Select] = Color.yellow;
        tileColors[(int)TileState.Attack] = Color.red;
        tileColors[(int)TileState.Skill] = Color.black;

        //movableColor = Color.blue;
    }
    public void Init(IntVect2D array)
    {
        positionArray.x = array.x;
        positionArray.y = array.y;
    }
    //選択したキャラクターの下
    public void ChangeColor(TileState tileState)
    {
        material.color = tileColors[(int)tileState];

    }

    // Update is called once per frame
    void Update()
    {

    }
}
