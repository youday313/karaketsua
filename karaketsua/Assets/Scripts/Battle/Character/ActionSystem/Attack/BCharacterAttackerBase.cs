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
        public List<BCharacterBase> attackTargetList = new List<BCharacterBase>();

        public bool IsNowAction { get; set;}


        //ターゲット選択済み
        //protected bool isSetTarget = false;
        public bool IsSetTarget
        {
            get { return attackTargetList.Count() != 0; }
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
            attackTargetList = new List<BCharacterBase>();
        }


        public virtual void Reset()
        {
            Disable();
            IsDone = false;

        }
        public virtual void OnCompleteAction()
        {
            if (OnComplete != null) OnComplete();
        }
        


    }
}