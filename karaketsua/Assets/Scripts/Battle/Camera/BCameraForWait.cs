using UnityEngine;
using System.Collections;
using Arbor;
using System.Collections.Generic;
using System.Linq;

public class BCameraForWait : BCameraBaseState {
	public StateLink[] nextState;
	Character activeCharacter;
	List<Character> characters;
	// Use this for initialization
	void Start () {

	}

	// Use this for enter state
	public override void OnStateBegin() {
		base.OnStateBegin();
		characters = GameObject.FindGameObjectsWithTag ("BattleCharacter").Select(x=>x.GetComponent<Character>()).ToList();
		SetActiveCharacter ();

		Move ();
	}
	void SetActiveCharacter()
	{
		activeCharacter = characters.FirstOrDefault (x => x.isNowSelect == true);

	}

	void Move(){

		Vector3 charaPosition = activeCharacter.transform.position;

		transform.position = CSTransform.CopyVector3(charaPosition + cameraTransform.position);
	}


	// Use this for exit state
	public override void OnStateEnd() {
		base.OnStateEnd();
	}

	// Update is called once per frame
	void Update () {

	}
}
