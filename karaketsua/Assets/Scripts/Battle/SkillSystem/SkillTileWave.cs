using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SkillTileWave : MonoBehaviour {

	public List<SkillTile> skillTiles=new List<SkillTile>();
	public SkillTile skillTilePrefab;
	CharacterSkill characterSkill;
    IntVect2D startPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Init(CharacterSkill _skill,IntVect2D _starPos)
    {
        characterSkill = _skill;
        startPos = _starPos;
    }
	
	
    //座標の修正が必要
    //キャラ位置から修正する
	public virtual void CreateNewTileSequence(IntVect2D arrayPos){
        //単数
        if (skillTiles.Count == 1)
        {
            CreateNewTile(arrayPos, _isStart: true, _isEnd: true, _preSkillTile: null);
        }
        //複数
        else
        {
            //最初
            var preTile=CreateNewTile(arrayPos, _isStart: true, _isEnd: false, _preSkillTile: null);
      //途中
            for (int i = 1; i < skillTiles.Count - 1; i++)
            {
                preTile=CreateNewTile(arrayPos, _isStart: false, _isEnd: false, _preSkillTile: preTile);
            
            }
            //最後
            CreateNewTile(arrayPos, _isStart: false, _isEnd: true, _preSkillTile: preTile);
        }
	}
    public SkillTile CreateNewTile(IntVect2D arrayPos,bool _isStart,bool _isEnd,SkillTile _preSkillTile)
    {
        var tile = Instantiate(skillTilePrefab) as SkillTile;
        tile.Init(this,arrayPos, _isStart, _isEnd, _preSkillTile);
        return tile;
    }

    public void SuccessDrag()
    {
        characterSkill.SuccessWave();
    }
    public void MissDrag()
    {

    }
}
