using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using BattleScene;

namespace BattleScene
{
    public enum CameraMode { FromBack = 0, FromFront, Up }
    public class BCameraManager : SingletonMonoBehaviour<BCameraManager>
    {
		[SerializeField] 
		private GameObject leanCameraObject;
        [SerializeField]
		private GameObject upCameraObject;
        [SerializeField]
        private GameObject moveAttackCameraObject;
        private GameObject moveCameraInstance;
        private BCameraMove leanCamera;
        // 現在のモード
        private CameraMode nowCameraMode;

        // 初期化
        public void Initialize()
        {
            leanCamera = leanCameraObject.GetComponent<BCameraMove>();
            leanCameraObject.SetActive(true);
            upCameraObject.SetActive(false);
            nowCameraMode = CameraMode.FromBack;
        }

        // ボタンから変更
        public void NextCameraModeFromButton()
        {
            var nextCameraMode = (CameraMode)(((int)nowCameraMode + 1) % Enum.GetNames(typeof(CameraMode)).Length);
            nowCameraMode = nextCameraMode;
            // 後ろから前
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
            tryResetMoveCamera();
            leanCameraObject.SetActive(false);
            upCameraObject.SetActive(true);
            nowCameraMode = CameraMode.Up;
        }

        public void ActiveLeanMode()
        {
            tryResetMoveCamera();
            leanCameraObject.SetActive(true);
            upCameraObject.SetActive(false);
        }

        // 移動攻撃のカメラをセット
        public void StartMoveAttack(Transform chara)
        {;
            moveCameraInstance = Instantiate(moveAttackCameraObject, moveAttackCameraObject.transform.localPosition, moveAttackCameraObject.transform.rotation) as GameObject;
            moveCameraInstance.transform.SetParent(chara, worldPositionStays:false);
            moveCameraInstance.SetActive(true);
            gameObject.SetActive(false);
        }

        private void tryResetMoveCamera()
        {
            if(moveCameraInstance != null) {
                Destroy(moveCameraInstance);
                moveCameraInstance = null;
            }
            gameObject.SetActive(true);
        }
    }
}