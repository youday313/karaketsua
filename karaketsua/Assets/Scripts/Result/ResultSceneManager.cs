using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ResultScene
{
    public class ResultSceneManager: MonoBehaviour
    {
        [SerializeField]
        private CharacterStatusManager statusPanel;
        [SerializeField]
        private Button nextButton; 

        void Start()
        {
            nextButton.onClick.AddListener(() => SceneManager.Instance.LoadScene(Scene.Battle));
        }
    }

}