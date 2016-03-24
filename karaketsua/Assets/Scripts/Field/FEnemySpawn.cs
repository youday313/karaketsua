using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FieldScene;

using UnityEngine.UI;

namespace FieldScene
{
    //敵との遭遇
    public class FEnemySpawn : MonoBehaviour
    {
        private AsyncOperation asyncOpe = null;
        private bool sceneChanged = false;  // 次のシーンに移行したかどうかのフラグ.
        public FCharacter character;

        public float spawnInterval;

        public Text text;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckSpwan();

        }
        void CheckSpwan()
        {

            if (character.MoveTime > spawnInterval && asyncOpe==null)
            {
                asyncOpe= UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");
                asyncOpe.allowSceneActivation = false;
            }

            if (asyncOpe != null)
            {
                if (!sceneChanged && asyncOpe.progress >= 0.9f)
                {
                    // 次のシーンに移行.
                    asyncOpe.allowSceneActivation = true;
                    sceneChanged = true;
        
                }
            }


        }






    }
}