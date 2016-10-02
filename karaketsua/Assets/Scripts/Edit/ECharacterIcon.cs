using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EditScene
{

    public class ECharacterIcon: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private Transform moveTemp;
        [SerializeField]
        private Image iconImage;

        private Vector3 oldPosition;
        private ETile nowTile = null;
        [System.NonSerialized]
        public IntVect2D vect2D = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);
        private bool isPlayer;

        // タイルに乗ったとき
        public Action OnChangeTile;
        // 決定時
        public Action Decide;

        // 初期化
        public void Initialize(CharacterMasterParameter chara, IntVect2D initPos = null, bool isPlayer = true)
        {
            var prefabName = "Character/ATB/ATB" + chara.charaName;
            var image = Resources.Load<Sprite>(prefabName);
            iconImage.sprite = image;
            gameObject.SetActive(true);
            this.isPlayer = isPlayer;
            if(initPos != null) {
                vect2D = initPos;
            }
            Decide += () => {
                if(isPlayer) {
                    vect2D.Log();
                    PlayerGameData.Instance.SelectPlayerChatacterIds.Add(chara.id);
                    PlayerGameData.Instance.BattlePlayerPosition.Add(chara.id, vect2D);
                }
                else {
                    PlayerGameData.Instance.SelectEnemyCharacterIds.Add(chara.id);
                    PlayerGameData.Instance.BattleEnemyPosition.Add(chara.id, vect2D);
                }
            };
            oldPosition = CSTransform.CopyVector3(rectTransform.position);
        }

        // ドラッグ開始
        public void OnBeginDrag(PointerEventData e)
        {
            if(!isPlayer) {
                return;
            }
            rectTransform.position = CSTransform.CopyVector3(e.position);
            transform.SetParent(moveTemp);
        }

        // ドラッグ中
        public void OnDrag(PointerEventData e)
        {
            if(!isPlayer) {
                return;
            }
            rectTransform.position = CSTransform.CopyVector3(e.position);
        }

        // ドラッグ終了
        public void OnEndDrag(PointerEventData e)
        {
            if(!isPlayer) {
                return;
            }
            //タイルの取得
            var t = ETileManager.Instance.GetTile(e.position);
            tryMove(t);
        }

        public void MoveOnTile()
        {
            var t = ETileManager.Instance.Tiles.Where(v => v.Vect.IsEqual(vect2D)).FirstOrDefault();
            tryMove(t);
        }
        // タイルの上に移動
        private void tryMove(ETile newTile)
        {
            //タイル外か他にいる
            var canMove = newTile != null && !newTile.IsOnCharacter;
            var toTile = canMove ? newTile : nowTile;
            // 移動
            move(toTile);
            if(OnChangeTile != null) {
                OnChangeTile();
            }
        }

        private void move(ETile t)
        {
            transform.SetParent(t.transform, worldPositionStays: false);
            rectTransform.localPosition = Vector2.zero;
            oldPosition = CSTransform.CopyVector3(rectTransform.position);
            vect2D = t.Vect;
            nowTile = t;
        }
    }
}
