using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{
    public class BCharacterAttackerBase : MonoBehaviour
    {
        protected BCharacterBase character;
        protected BCharacterAnimator animator;

        public event Action OnComplete;

        //ターゲット
        [System.NonSerialized]
        public List<BCharacterBase> TargetList = new List<BCharacterBase>();

        public bool IsNowAction { get; set;}


        //ターゲット選択済み
        //protected bool isSetTarget = false;
        public bool IsSetTarget
        {
            get { return TargetList.Count() != 0; }
        }

        public bool IsDone { get; set;}



        // Use this for initialization
        public virtual void Awake()
        {
            character = GetComponent<BCharacterBase>();
            animator = GetComponent<BCharacterAnimator>();
        }


        //行動可能時に有効化
        protected bool isEnable = false;
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                if (value == true)
                {
                    Enable();
                }
                else if (value == false)
                {
                    Disable();
                }
                isEnable = value;
            }
        }


        public virtual void Enable()
        {
            IsDone = false;
        }
        public virtual void Disable()
        {
            TargetList = new List<BCharacterBase>();
        }


        public virtual void Reset()
        {
            Disable();
            IsDone = false;

        }

        protected void onCompleteAction()
        {
            character.ResetRotate();
            foreach(var tar in TargetList.Where(x => x != null)) {
                tar.ResetRotate();
            }
            TargetList = new List<BCharacterBase>();
            if(OnComplete != null) {
                OnComplete();
            }
        }

        protected void HideOtherCharacters()
        {
            // 関係のないキャラクター非表示
            var showCharacters = new List<BCharacterBase>();
            showCharacters.Add(character);
            showCharacters.AddRange(TargetList);
            // showCharacters以外は非表示
            BCharacterManager.Instance.HideCharacter(showCharacters);
        }

        // キャラ同士を向かい合わせる
        protected void FaceCharacter()
        {
            // 向く方向を変える
            // 攻撃側
            if(TargetList.Count == 1) {
                var targetPos = TargetList[0].transform.position;
                targetPos.y = transform.position.y;
                transform.LookAt(targetPos);
            }
            // ターゲット側
            foreach(var target in TargetList) {
                var attackerPos = transform.position;
                attackerPos.y = target.transform.position.y;
                target.transform.LookAt(attackerPos);
            }
        }
        // 技倍率
        // critical * randam * except
        private const float CriticalAmplitude = 1.5f;
        private const float RandomMaxRange = 1.2f;
        private const float RandomMinRange = 0.8f;
        protected float calcBaseDamageRate()
        {
            //会心＊振れ幅＊技倍率＊タップ倍率
            // 仮：知力はクリティカル発生率とする
            var critical = character.characterParameter.intellisense >= (UnityEngine.Random.value * 100) ? CriticalAmplitude : 1;
            var random = UnityEngine.Random.Range(RandomMinRange, RandomMaxRange);
            // TODO:状態異常による影響値をかける
            return critical * random;
        }
    }
}