using UnityEngine;
using UnityEngine.UI;

using EditScene;

namespace EditScene
{
    public class ETile: MonoBehaviour
    {
        [SerializeField]
        private Image tileImage;
        [SerializeField]
        private Sprite attackSprite;    // 赤色タイル

        public IntVect2D vect { get; private set; }
        public bool isAttachable { get; private set; }

        // 初期化
        public void Initialize(IntVect2D v, bool _isAttachable)
        {
            vect = v;
            isAttachable = _isAttachable;
            if(!isAttachable) {
                return;
            }
            tileImage.sprite = attackSprite;
        }
    }
}