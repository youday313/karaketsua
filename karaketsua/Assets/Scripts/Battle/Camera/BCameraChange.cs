using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BattleScene
{
    //public enum CameraMode { FromBack = 0, FromFront, Up }
    public class BCameraChange : Singleton<BCameraChange>
    {

        public GameObject leanCameraObject;
        public GameObject upCameraObject;
        CameraMove leanCamera;
        //現在のモード
        public CameraMode nowCameraMode;
        // Use this for initialization
        void Start()
        {

            leanCamera = leanCameraObject.GetComponent<CameraMove>();
            leanCameraObject.SetActive(true);
            upCameraObject.SetActive(false);
            nowCameraMode = CameraMode.FromBack;
        }

        //ボタンから変更
        public void NextCameraModeFromButton()
        {
            var nextCameraMode = (CameraMode)(((int)nowCameraMode + 1) % Enum.GetNames(typeof(CameraMode)).Length);
            nowCameraMode = nextCameraMode;
            //後ろから前
            if (nextCameraMode == CameraMode.FromFront || nextCameraMode == CameraMode.FromBack)
            {
                ActiveLeanMode();
                leanCamera.ChangeFrontMode(nextCameraMode);
            }
                //上
            else if(nextCameraMode==CameraMode.Up)
            {
                ActiveUpMode();
            }
        }

        public void ActiveUpMode()
        {
            leanCameraObject.SetActive(false);
            upCameraObject.SetActive(true);
        }

        public void ActiveLeanMode()
        {
            leanCameraObject.SetActive(true);
            upCameraObject.SetActive(false);

        }
    }
}