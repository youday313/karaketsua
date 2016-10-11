using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;
using System;

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
        public virtual void OnCompleteAction()
        {
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
    }
}