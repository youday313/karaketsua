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

using BattleScene;

namespace BattleScene
{

    public class BActiveTime : MonoBehaviour
    {

        //イベントで通知
        //キャラクター行動選択時
        public event Action<BActiveTime> OnStopActiveTimeE;
        //WaitTimerが動く時
        public event Action<BActiveTime> OnStartActiveTimeE;

        //関連付けられたキャラ
        [System.NonSerialized]
        public BCharacterPlayer character;

        Slider slider;
        [SerializeField]
        Image iconImage;
        float activeSpeed;
        //public float initTimeValue=20;
        //最大速度のキャラがかかる秒数
        static float cycleSecond = 3f;
        static float minOneCycleValue;
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
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        void Awake()
        {
            slider = GetComponent<Slider>();
            //slider.value = initTimeValue;
            isActive = true;
        }
        public void Init(BCharacterPlayer chara)
        {
            character = chara;
            activeSpeed = character.characterParameter.activeSpeed;
            iconImage.sprite = Resources.Load<Sprite>("Icon/" + character.characterParameter.charaName);
            //slider.maxValue = initTimeValue;
            //nowWaitTime = initTimeValue;

        }

        public void StartWave()
        {
            isActive = true;
        }

        void Start()
        {
            SetActiveTimeValue();
            BSceneState.Instance.StartWave += StartWave;
        }
        void SetActiveTimeValue()
        {
            //最大速度取得
            var maxSpeed = GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<BActiveTime>().activeSpeed).Max();
            //最小でかかるスライダー量取得
            minOneCycleValue = maxSpeed * cycleSecond;
            //max補正
            slider.maxValue = minOneCycleValue;
            nowWaitTime = minOneCycleValue;
        }

        void Update()
        {
            if (isActive)
            {
                UpdateValue();
            }
        }

        void UpdateValue()
        {
            NowWaitTime = NowWaitTime - Time.deltaTime * CalcDecreaseSpeedFromActiveSpeed();
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
            foreach (var time in GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<BActiveTime>()).Where(x => x.character != this))
            {
                time.IsActive = false;
            }
        }

        //行動終了とActiveTime再開
        public void ResetValue()
        {
            NowWaitTime = minOneCycleValue;
            //OnStartActiveTimeE(this);
            //activeTime再開
            foreach (var time in GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<BActiveTime>()))
            {
                time.IsActive = true;
            }
        }
        public void DeathCharacter()
        {
            Destroy(this.gameObject);
        }


    }
}