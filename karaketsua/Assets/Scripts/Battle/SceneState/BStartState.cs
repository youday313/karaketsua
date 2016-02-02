using UnityEngine;
using System.Collections;
using Arbor;
using System;

public class BStartState : StateBehaviour {
	// Use this for initialization
	public event Action OnStartSceneE;


    void Start () {
	
	}

	// Use this for enter state
	public override void OnStateBegin() {
        OnStartSceneE();
	}

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
