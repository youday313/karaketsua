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
using System;


public class ActiveTime : MonoBehaviour
{

    //イベントで通知
    //キャラクター行動選択時
    public event Action<ActiveTime> OnStopActiveTimeE;
    //WaitTimerが動く時
    public event Action<ActiveTime> OnStartActiveTimeE;

	//関連付けられたキャラ
	public Character character;
	//private

    //他のWaitTime
    //static List<ActiveTime> allActiveTimes=new List<ActiveTime>();

	Slider slider;
    float activeSpeed;
    float initTimeValue=100;
    float nowWaitTime;
    float NowWaitTime
    {
        get { return nowWaitTime; }
        set
        {
            nowWaitTime = Mathf.Max(0, value);
            slider.value = nowWaitTime;
        }
    }

    //動いている
	bool isActive;
	public bool IsActive {
		get { return isActive; }
		set{ isActive = value;}
	}

	void Awake ()
	{
        slider = GetComponent<Slider>();
        slider.value = initTimeValue;
        isActive = true;
	}
	public void Init(Character chara)
    {
        activeSpeed = chara.characterParameter.activeSpeed;
        character = chara;
        slider.maxValue = initTimeValue;
        nowWaitTime = initTimeValue;

    }

	void Update ()
	{
		if (isActive) {
			UpdateValue ();	
		}
    }

	void UpdateValue(){
        NowWaitTime =NowWaitTime- Time.deltaTime * CalcDecreaseSpeedFromActiveSpeed();
        if (NowWaitTime == 0)
            {
                StopActive();
            }
        }

    float CalcDecreaseSpeedFromActiveSpeed()
    {
        return activeSpeed;
    }
    //行動キャラ選択
    void StopActive()
    {
        if (OnStopActiveTimeE != null)
        {
            //イベント通知
            OnStopActiveTimeE(this);
        }
        //他のActiveTime停止
        foreach (var time in GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<ActiveTime>()).Where(x=>x.character!=this))
        {
           time.IsActive=false;
        }
    }

    //行動終了とActiveTime再開
    public void ResetValue()
    {
        NowWaitTime = initTimeValue;
        //OnStartActiveTimeE(this);
        //activeTime再開
        foreach (var time in GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<ActiveTime>()))
        {
            time.IsActive = true;
        }
    }
    public void DeathCharacter()
    {
        Destroy(this.gameObject);
    }
 
	
}