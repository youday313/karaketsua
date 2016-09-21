﻿using UnityEngine;
using UnityEngine.UI;

using EditScene;

namespace EditScene
{
    public class ETile: MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private Image tileImage;
        [SerializeField]
        private Sprite attackSprite;    // 赤色タイル


        public IntVect2D Vect { get; private set; }
        public bool IsAttachable { get; private set; }
        public bool IsOnCharacter { get; set; }

        // 初期化
        public void Initialize(IntVect2D v, bool _isAttachable)
        {
            Vect = v;
            IsAttachable = _isAttachable;
            if(!IsAttachable) {
                return;
            }
            tileImage.sprite = attackSprite;
        }

        // 指定点を含むか
        public bool IsContain(Vector2 pos)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos) && IsAttachable;
        }
    }
}