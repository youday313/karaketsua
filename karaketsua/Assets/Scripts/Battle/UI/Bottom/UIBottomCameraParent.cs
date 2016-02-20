using UnityEngine;
using System.Collections;
using BattleScene;

namespace BattleScene
{

    public class UIBottomCameraParent : UIBottomBase
    {

        public UIBottomCameraChange cameraChange;
        public UIBottomCameraReset cameraReset;
        
        public GameObject cameraChangeObject;
        public GameObject cameraResetObject;

        public UIBottomCommandParent commad;

        // Use this for initialization
        void Start()
        {
            //cameraChangeObject = cameraChange.gameObject;
            //cameraResetObject = cameraReset.gameObject;


            commad.UpdateCameraUIMode += Reset;
            BCameraMove.Instance.MoveCamera += MoveCamera;
            BSceneState.Instance.StartWave += StartWave;
        }

        public void StartWave()
        {
            cameraChangeObject.SetActive(true);
            cameraResetObject.SetActive(false);
        }

        // Update is called once per frame
        public override void UpdateUI()
        {
            cameraChangeObject.SetActive(true);
            cameraResetObject.SetActive(false);

            cameraChange.UpdateUI();
            cameraReset.UpdateUI();
        }

        public void Reset()
        {
            BCameraMove.Instance.ResetCamera();
            cameraChangeObject.SetActive(true);
            cameraResetObject.SetActive(false);

            cameraChange.UpdateUI();
            cameraReset.UpdateUI();
        }
        public void MoveCamera()
        {
            cameraChangeObject.SetActive(false);
            cameraResetObject.SetActive(true);

            cameraChange.UpdateUI();
            cameraReset.UpdateUI();
        }


    }
}