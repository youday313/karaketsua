using UnityEngine;
using System.Collections;
using Arbor;

public class BCameraStartState : StateBehaviour {
	public StateLink nextState;

	// Use this for initialization
	void Start () {
		Transition (nextState,true);
	}
	// Use this for exit state
	public override void OnStateBegin() {
		
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
