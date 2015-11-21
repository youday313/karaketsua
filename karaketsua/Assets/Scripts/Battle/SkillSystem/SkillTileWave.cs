using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SkillTileWave : MonoBehaviour {

	List<SkillTile> skillTiles=new List<SkillTile>();
	public SkillTile skillTilePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void CreateNewTile(){
		foreach (var skillTile in skillTiles) {
			var tile =Instantiate (skillTilePrefab);
			tile.
		}
	}
}
