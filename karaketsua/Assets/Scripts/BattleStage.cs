//BattleStage
//作成日
//<summary>
//TileBass管理
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BattleStage : Singleton<BattleStage>
{

    public static readonly int stageSizeX = 2;
    public static readonly int stageSizeY = 3;

    List<TileBase> tileBases = new List<TileBase>();
    // Use this for initialization
    void Start()
    {
        // 配置するプレハブの読み込み 
        var prefab = Resources.Load<TileBase>("Tile");
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
                    var tile =Instantiate(prefab,tile_pos, Quaternion.identity) as TileBase;
                    tile.Init(new IntVect2D(i,j));

                    // 生成元の下に複製したプレハブをくっつける 
                    tile.transform.parent = stageObject.transform;

                    //リストに格納
                    tileBases.Add(tile);
                }
            }
        }
    }

    public void UpdateTileColors(IntVect2D positionArray,TileState state)
    {
        //OnSelect
        if (state == TileState.Active)
        {
            ResetTileColor();
            ChangeColor(positionArray, state);
            ChangeNeighborTilesColor(positionArray, TileState.Movable);
        }
        //Move
        else if (state == TileState.Moved)
        {
            ResetTileColor();
            ChangeColor(positionArray, state);
        }
        else if (state == TileState.Target) {
            ChangeColor(positionArray, state);
        }
        else if (state == TileState.Attackable)
        {
            ResetTileColor();
            ChangeColor(positionArray, TileState.Active);
            ChangeNeighborTilesColor(positionArray, TileState.Attackable);
        }
        else if (state == TileState.Skill)
        {
            ChangeColor(positionArray, state);
        }
        

        //TargetSelect

        
        //

    }

    //ゲット関連
    public TileBase GetTile(IntVect2D position)
    {
        return tileBases.Where(t => position.IsEqual(t.positionArray)).FirstOrDefault();
    }

    public List<TileBase> GetVerticalHorizontalTiles(IntVect2D position)
    {

        return GetTilesFormDistance(position, 1f);
    }
    public List<TileBase> GetTilesFormDistance(IntVect2D position , float distance)
    {
        return tileBases.Where(t => 
        (Vector2.Distance(new Vector2(t.positionArray.x, t.positionArray.y), new Vector2(position.x, position.y)) <= distance)
        && !(position.IsEqual(t.positionArray))).ToList();
    }

    //色変更
    public void ResetTileColor()
    {
        foreach (var tile in tileBases)
        {
            tile.ChangeColor(TileState.Default);
        }
    }
    public void ChangeColor(IntVect2D position,TileState state,bool reset=false)
    {
        if (reset) ResetTileColor();
        GetTile(position).ChangeColor(state);


    }
    //上下左右のタイル色変更
    public void ChangeNeighborTilesColor(IntVect2D position, TileState toState, bool reset=false)
    {
        if (reset) ResetTileColor();
        foreach(var tile in GetVerticalHorizontalTiles(position))
        {
            tile.ChangeColor(toState);
        }

    }
    public void ChangeTilesColorFromDistance(IntVect2D position, TileState toState,float distance, bool reset = false)
    {
        if (reset) ResetTileColor();
        foreach (var tile in GetTilesFormDistance(position,distance))
        {
            tile.ChangeColor(toState);
        }

    }
}