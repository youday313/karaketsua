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
//Active:行動キャラ,Moved:移動終了,Movable:移動可能範囲,Target:攻撃先,Attackble:攻撃範囲,Skill:スキル範囲,Skilled;スキル塗り終わり
//public enum TileState { Default=0,Active,Moved,Movable,Target,Attackable,Skill,Skilled};
public enum TileState{Default,Attack,Move,Skill};

public class TileBase : MonoBehaviour
{

    //[SerializeField]
    //Color[] tileColors=new Color[Enum.GetNames(typeof(TileState)).Count()];
    public Material[] tileMaterials;
    public IntVect2D positionArray = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);//配列番号
    
    Renderer renderer;

    void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        //renderer = gameObject.GetComponent<Renderer>();
        // 選択時と非選択時のカラーを保持 
        //tileColors[(int)TileState.Default] = material.color;
        //tileColors[(int)TileState.Default] = new Color(material.color.r, material.color.g, material.color.b);
        //tileColors[(int)TileState.Active] = Color.red;
        //tileColors[(int)TileState.Moved] = Color.green;
        //tileColors[(int)TileState.Movable] = Color.cyan;
        //tileColors[(int)TileState.Target] = Color.yellow;
        //tileColors[(int)TileState.Attackable] = Color.magenta;
        //tileColors[(int)TileState.Skill] = Color.black;
        //tileColors[(int)TileState.Skilled] = Color.white;


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
        renderer.material = tileMaterials[(int)tileState];

    }


    //スクリーン座標を受け取る
    public static IntVect2D GetArrayFromRay(Vector2 touchPosition)
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ray = camera.ScreenPointToRay(touchPosition);
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

}
