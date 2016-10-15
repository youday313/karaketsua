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

    public class BActiveTime: MonoBehaviour
    {

        //イベントで通知
        //キャラクター行動選択時
        public event Action OnStopActiveTimeE;

        //関連付けられたキャラ
        [System.NonSerialized]
        public BCharacterBase Character;

        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Image iconImage;
        private float activeSpeed;
        //public float initTimeValue=20;
        //最大速度のキャラがかかる秒数
        const float cycleSecond = 3f;
        static float minOneCycleValue;
        float nowWaitTime;
        float NowWaitTime {
            get { return nowWaitTime; }
            set {
                nowWaitTime = Mathf.Max(0, value);
                slider.value = nowWaitTime;
            }
        }

        //動いている
        public bool IsActive {
            get; set;
        }
        // 初期化
        public void initialize(BCharacterBase chara)
        {
            Character = chara;
            activeSpeed = Character.characterParameter.activeSpeed;
            iconImage.sprite = Resources.Load<Sprite>(ResourcesPath.ATBIcon + Character.characterParameter.charaName);
            Character.OnDeathE += Delete;
            //slider.maxValue = initTimeValue;
            //nowWaitTime = initTimeValue;

        }

        public void StartWave()
        {
            IsActive = true;
        }

        void Start()
        {
            IsActive = true;
            SetActiveTimeValue();
            BSceneState.Instance.StartWave += StartWave;
            Character.OnEndActiveE += ResetValue;
            BCharacterBase.OnEndActiveStaticE += Resume;
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
            if(IsActive) {
                UpdateValue();
            }
        }

        void UpdateValue()
        {
            NowWaitTime = NowWaitTime - Time.deltaTime * CalcDecreaseSpeedFromActiveSpeed();
            if(NowWaitTime == 0) {
                OnActive();
            }
        }

        float CalcDecreaseSpeedFromActiveSpeed()
        {
            return activeSpeed;
        }
        //行動キャラ選択
        void OnActive()
        {
            Character.OnActive();

            //他のActiveTime停止
            foreach(var time in GameObject.FindGameObjectsWithTag("ActiveTime").Select(x => x.GetComponent<BActiveTime>()).Where(x => x.Character != this)) {
                time.IsActive = false;
            }
        }

        //行動終了とActiveTime再開
        public void ResetValue(BCharacterBase _activeCharacter)
        {
            NowWaitTime = minOneCycleValue;

        }
        public void Resume()
        {
            IsActive = true;
        }
        public void Delete(BCharacterBase chara)
        {
            chara.OnEndActiveE -= ResetValue;
            BCharacterBase.OnEndActiveStaticE -= Resume;

            Destroy(this.gameObject);
        }


    }
}