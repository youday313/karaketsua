using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EditScene
{

    public class ECharacterIcon: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private Image iconImage; 

        private Vector3 oldPosition;
        private ETile onTile = null;
        [System.NonSerialized]
        public IntVect2D vect2D = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);

        // 初期化
        public void Initialize(string charaName, IntVect2D initPos = null)
        {
            var prefabName = "Character/ATB/ATB" + charaName;
            var image = Resources.Load<Sprite>(prefabName);
            iconImage.sprite = image;
            gameObject.SetActive(true);

            if(initPos != null) {
                vect2D = initPos;
            }
            oldPosition = CSTransform.CopyVector3(rectTransform.position);
        }

        // ドラッグ開始
        public void OnBeginDrag(PointerEventData e)
        {
            rectTransform.position = CSTransform.CopyVector3(e.position);
            //obj.SetAsFirstSibling();
        }

        // ドラッグ中
        public void OnDrag(PointerEventData e)
        {
            rectTransform.position = CSTransform.CopyVector3(e.position);
        }

        // ドラッグ終了
        public void OnEndDrag(PointerEventData e)
        {
            //タイルの取得
            var tile = ETileManager.Instance.GetTile(e.position);

            //タイル外
            if(tile == null) {
                rectTransform.position = oldPosition;
                return;
            }
            // 他キャラクターがすでにいる
            if(tile.isOnCharacter) {
                rectTransform.position = oldPosition;
                return;
            }

            // 移動
            rectTransform.position = CSTransform.CopyVector3(tile.GetComponent<RectTransform>().position);
            oldPosition = CSTransform.CopyVector3(rectTransform.position);
            vect2D = tile.vect;
            onTile = tile;
        }
    }
}
