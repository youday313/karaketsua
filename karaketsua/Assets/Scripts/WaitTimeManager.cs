using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaitTimeManager : Singleton<WaitTimeManager> {

	List<WaitTime> waitTimes=new List<WaitTime>();
	CameraMove cameraMove;

	// Use this for initialization
	void Start () {
	
		foreach (var waitTime in GameObject.FindGameObjectsWithTag("WaitTime")) {
			waitTimes.Add (waitTime.GetComponent<WaitTime>());
		}
		cameraMove = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraMove>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//waitTimeから呼びアクティブなものを選択
	public void OnActiveCharacter(){
		foreach (var wait in waitTimes) {
			wait.IsActive = false;
		}
		cameraMove.MoveToBack ();
	}


}
