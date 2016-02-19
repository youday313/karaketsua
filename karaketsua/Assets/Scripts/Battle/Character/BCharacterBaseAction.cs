using UnityEngine;
using System.Collections;

namespace BattleScene
{
    public class BCharacterBaseAction : MonoBehaviour
    {

        protected BCharacter character;
        protected BCharacterAnimator animator;
        protected CameraMove cameraMove;
        //行動可能時に有効化
        protected bool isEnable = false;
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                if (isEnable == false && value == true)
                {
                    Enable();
                }
                else if (isEnable == true && value == false)
                {
                    Disable();
                }
                isEnable = value;
            }

        }
        [System.NonSerialized]
        public bool isNowAction = false;



        // Use this for initialization
        void Start()
        {
            Init();
        }
        //作成時に初期化処理
        public virtual void Init()
        {
            character = GetComponent<BCharacter>();
            animator = GetComponent<BCharacterAnimator>();

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