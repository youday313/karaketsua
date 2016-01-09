using UnityEngine;
using System.Collections;

public class SkillLeftToRightAllHorizontal : SkillTileWave {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //座標の修正が必要
    //キャラ位置から修正する
    public void CreateNewTileSequence(IntVect2D arrayPos)
    {
        ////単数
        //if (skillTiles.Count == 1)
        //{
        //    CreateNewTile(new IntVect2D(-BattleStage.stageSizeX,arrayPos.y), _isStart: true, _isEnd: true, _preSkillTile: null);
        //}
        ////複数
        //else
        //{
            //最初
            var preTile = CreateNewTile(new IntVect2D(-BattleStage.stageSizeX,arrayPos.y), _isStart: true, _isEnd: false, _preSkillTile: null);
            //途中
            for (int i = 1; i < skillTiles.Count - 1; i++)
            {
                preTile = CreateNewTile(new IntVect2D(-BattleStage.stageSizeX+i,arrayPos.y), _isStart: false, _isEnd: false, _preSkillTile: preTile);

            }
            //最後
            CreateNewTile(new IntVect2D(BattleStage.stageSizeX,arrayPos.y), _isStart: false, _isEnd: true, _preSkillTile: preTile);
        //}
    }
}
