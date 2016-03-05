using UnityEngine;
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

        //ターゲット
        [System.NonSerialized]
        public List<BCharacterBase> attackTarget = new List<BCharacterBase>();

        [System.NonSerialized]
        protected bool isNowAction = false;
        public bool IsNowAction
        {
            get { return isNowAction; }
        }

        //ターゲット選択済み
        //protected bool isSetTarget = false;
        public bool IsSetTarget
        {
            get { return attackTarget.Count() != 0; }
        }

        [System.NonSerialized]
        protected bool isDone = false;
        public bool IsDone
        {
            get { return isDone; }
        }


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
            isDone = false;
        }
        public virtual void Disable()
        {
            attackTarget = new List<BCharacterBase>();
        }


        public virtual void Reset()
        {
            Disable();
            isDone = false;

        }


    }
}