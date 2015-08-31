//BattleStage
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleStage : BattleBase
{

    protected int _sceneTask;
    public static readonly int stageSizeX = 2;
    public static readonly int stageSizeY = 3;


    // Use this for initialization
    void Start()
    {
        // 配置するプレハブの読み込み 
        var prefab = Resources.Load<GameObject>("Tile");
        // 配置元のオブジェクト指定 
        var stageObject = GameObject.FindWithTag("Stage");
        // タイル配置
        for (int i = -stageSizeX; i <= stageSizeX; i++)
        {
            for (int j = -stageSizeY; j <= stageSizeY; j++)
            {

                Vector3 tile_pos = new Vector3(
                    0 + prefab.transform.localScale.x * i ,
                    0,
                    0 + prefab.transform.localScale.z * j 

                  );

                if (prefab != null)
                {
                    // プレハブの複製 
                    var tile =Instantiate(prefab,tile_pos, Quaternion.identity) as GameObject;
                    tile.GetComponent<TileBase>().Init(new Vect2D<int>(i,j));

                    // 生成元の下に複製したプレハブをくっつける 
                    tile.transform.parent = stageObject.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}