using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaitTimeManager : Singleton<WaitTimeManager> {

	List<WaitTime> waitTimes=new List<WaitTime>();
    public WaitTime waitTimePrefab;
    public Transform backGazeCanvas;

	// Use this for initialization
	void Start () {
	
		//foreach (var waitTime in GameObject.FindGameObjectsWithTag("WaitTime")) {
		//	waitTimes.Add (waitTime.GetComponent<WaitTime>());
		//}
		
       
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

    public void RestartWaitTime()
    {
        foreach (var wait in waitTimes)
        {
            wait.IsActive = true;
        }

    }

    public WaitTime CreateWaitTime(float waitSpeed,Character chara)
    {
        var wTime= Instantiate(waitTimePrefab) as WaitTime;
        wTime.Init(waitSpeed,chara);
        waitTimes.Add(wTime);
        wTime.transform.SetParent(backGazeCanvas,false);
        return wTime;
    }


}
