using UnityEngine;
using System.Collections;

namespace BattleScene
{
    public class BattleSceneManager: MonoBehaviour
    {


        [SerializeField]
        private BCharacterManager characterManager;
        [SerializeField]
        private BCameraManager cameraManager;
        [SerializeField]
        private UIBottomAllManager uiManager;

        void Awake()
        {
            // キャラクター作成
            characterManager.Initialze();
            // カメラセット
            cameraManager.Initialize();
            // UIセット
            uiManager.initialize();



        }

    }

}