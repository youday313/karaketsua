using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace EditScene
{
    public class EBattleStartButton: MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public void Initialize(List<ECharacterIcon> icons)
        {
            //ボタン有効無効
            foreach(var icon in icons) {
                icon.OnChangeTile += () => {
                    button.interactable = icons.Where(x => IntVect2D.IsNull(x.vect2D) == true).Count() == 0;
                };
            }

            // 保存
            button.onClick.AddListener(() => {
                PlayerGameData.Instance.ResetPositionList();
                foreach(var icon in icons) {
                    icon.Decide();
                }
                SceneManager.Instance.LoadNextScene(Scene.Battle);
            });
        }
    }
}