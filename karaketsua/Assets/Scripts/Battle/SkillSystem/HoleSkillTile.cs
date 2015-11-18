using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HoleSkillTile : MonoBehaviour {


	public List<SkillTileWave> skillTileWaves=new List<SkillTileWave>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ICreateNewWave(){
		foreach (var wave in skillTileWaves) {
			CreateNewWave (wave);

			yield return null;
		}
	}
	void CreateNewWave(SkillTileWave wave){
		wave.CreateNewTile ();
	}
}
