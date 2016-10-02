using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EditScene
{
    public class EBattleStartButton: MonoBehaviour
    {
        [SerializeField]
        private Button button;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="icons">プレイヤーのアイコン</param>
        /// <param name="callback">ボタンタップ時コールバック</param>
        public void Initialize(List<ECharacterIcon> icons, Action callback)
        {
            //ボタン有効無効
            foreach(var icon in icons) {
                icon.OnChangeTile += () => {
                    button.interactable = icons.Where(x => IntVect2D.IsNull(x.vect2D) == true).Count() == 0;
                };
            }

            // 保存
            button.onClick.AddListener(() => callback());
        }
    }
}