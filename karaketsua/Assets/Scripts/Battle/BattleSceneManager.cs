using UnityEngine;
using System.Collections;
using System;

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
        [SerializeField]
        private GameObject battleStartAnimation;
        [SerializeField]
        private AnimationCallbacker battleEndAnimation;

        void Start()
        {
            // キャラクター作成
            characterManager.Initialze();
            // カメラセット
            cameraManager.Initialize();
            // UIセット
            uiManager.initialize();
            battleStartAnimation.SetActive(true);
            BCharacterBase.OnDeathStaticE += () => {
                battleEndAnimation.Onfinish += () => SceneManager.Instance.LoadScene(Scene.Result);
                battleEndAnimation.gameObject.SetActive(true);
            };
        }
    }
}