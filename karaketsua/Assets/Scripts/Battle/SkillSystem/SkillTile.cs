using UnityEngine;
using System.Collections;

public class SkillTile : MonoBehaviour {

	bool isStartTile=false;

	IntVect2D arrayPosition = new IntVect2D (IntVect2D.nullNumber, IntVect2D.nullNumber);//配列番号

	// Use this for initialization
	void Start () {
	
	}
	public void Init(IntVect2D _arrayPosition){
		arrayPosition = _arrayPosition;
		UpdatePosition ();
	}
	void UpdatePosition(){
		var realX = transform.localScale.x * (arrayPosition - BattleStage.stageSizeX);
		var realZ = transform.localScale.z * (arrayPosition - BattleStage.stageSizeY);

		transform.position = new Vector3 (realX,0,realZ);

	}

	// Update is called once per frame
	void Update () {
	
	}

}
