using UnityEngine;
using System.Collections;

namespace BattleScene
{
    public class BCharacterActionBase : MonoBehaviour
    {

        protected BCharacterBase character;
        protected BCharacterAnimator animator;
        //protected BCameraMove cameraMove;
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
        [System.NonSerialized]
        protected bool isNowAction = false;



        // Use this for initialization
        public void Awake()
        {
            character = GetComponent<BCharacterBase>();
            animator = GetComponent<BCharacterAnimator>();
        }
        //作成時に初期化処理
        public virtual void Init()
        {


        }

        public virtual void Enable()
        {      
            isNowAction = false;
        }
        public virtual void Disable()
        {

        }
        //キャラクターの全行動終了
        public virtual void Reset()
        {

        }
    }

}