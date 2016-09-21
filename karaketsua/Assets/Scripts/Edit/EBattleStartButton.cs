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

        public void Initialize(List<ECharacterIcon> icons)
        {
            characterIcons.AddRange(icons);
            icon.ChangeOnTile += () => {
                //ボタン有効無効
                button.interactable = characterIcons.Where(x => IntVect2D.IsNull(x.vect2D) == true).Count() == 0;
            };
            button.onClick.AddListener(() => {
                SceneManager.Instance.SetNextScene("MainScene");
            });
        }
    }
}