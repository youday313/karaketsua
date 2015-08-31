using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaitTimeManager : Singleton<WaitTimeManager> {

	List<WaitTime> waitTimes=new List<WaitTime>();

	// Use this for initialization
	void Start () {
	
		foreach (var waitTime in GameObject.FindGameObjectsWithTag("WaitTime")) {
			waitTimes.Add (waitTime.GetComponent<WaitTime>());
		}
		
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//waitTimeから呼びアクティブなものを選択
	public void OnActiveCharacter(){
		foreach (var wait in waitTimes) {
			wait.IsActive = false;
		}

	}


}
