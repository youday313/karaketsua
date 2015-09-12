//WaitTime
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class WaitTime : MonoBehaviour
{



	//public

	//関連付けられたキャラ
	public Character character;
	//private

	Scrollbar scrollbar;
	WaitTimeManager waitTimeManager;
    public float waitSpeed;


	bool isActive;
	public bool IsActive {
		get { return isActive; }
		set{ isActive = value;}
	}

	void Start ()
	{
		scrollbar=GetComponent<Scrollbar>();
		waitTimeManager = WaitTimeManager.Instance;
		isActive = true;
        //StartCoroutine("Move");
	}
	
	void Update ()
	{
		if (isActive) {
			UpdateValue ();	
		}
    }

	void UpdateValue(){
		scrollbar.value -= Time.deltaTime*waitSpeed;
		if (scrollbar.value == 0) {
            //キャラクター選択時全体で共通動作はManagerで操作
			waitTimeManager.OnActiveCharacter ();
            //キャラクター個別に関連した処理
			character.OnSelect ();
		}
	}

    public void ResetValue()
    {
        waitTimeManager.RestartWaitTime();

    }

 
	
}