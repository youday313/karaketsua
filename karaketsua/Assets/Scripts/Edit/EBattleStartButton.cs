using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace EditScene
{
    public class EBattleStartButton: MonoBehaviour
    {
        private List<ECharacterIcon> characterIcons = new List<ECharacterIcon>();
        [SerializeField]
        private Button button;


        void Update()
        {
            //ボタン有効無効
            button.interactable = characterIcons.Where(x => IntVect2D.IsNull(x.vect2D) == true).Count() == 0;

        }

        public void SetNextScene()
        {
            //BattleScene.BStageData.Instance.SetCharacterData();
            SceneManager.Instance.SetNextScene("MainScene");
        }
    }
}