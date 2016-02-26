using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BattleScene;

namespace BattleScene
{

    public class BCharacterActionManagerBase : MonoBehaviour
    {

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

        public virtual bool IsNowAction()
        {
            return false;
        }

        public virtual bool IsDone()
        {
            return false;
        }


        public virtual void Enable()
        {

        }

        public virtual void Disable()
        {

        }
        public virtual void Reset()
        {
        }


    }

}