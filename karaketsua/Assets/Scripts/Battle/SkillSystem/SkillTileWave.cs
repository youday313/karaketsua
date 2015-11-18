using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SkillTileWave : MonoBehaviour {

	List<SkillTile> skillTiles=new List<SkillTile>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ICreateNewTile(){
		foreach (var tile in skillTiles) {
			CreateNewTile (tile);
			yield return null;
		}
	}
	void CreateNewTile(SkillTile tile){
		
	}
}
