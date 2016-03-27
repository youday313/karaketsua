using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EditScene
{

    public class ECharacterIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [System.NonSerialized]
        public Vector3 oldPosition;
        RectTransform rectTransform;
        [System.NonSerialized]
        public IntVect2D vect2D = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);

        // Use this for initialization
        void Start()
        {
            vect2D = new IntVect2D(IntVect2D.nullNumber, IntVect2D.nullNumber);
            rectTransform = GetComponent<RectTransform>();
            oldPosition = CSTransform.CopyVector3(rectTransform.position);

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnBeginDrag(PointerEventData e)
        {
            rectTransform.position = CSTransform.CopyVector3(e.position);
            //obj.SetAsFirstSibling();
        }
        public void OnDrag(PointerEventData e)
        {
            rectTransform.position = CSTransform.CopyVector3(e.position);
        }
        public void OnEndDrag(PointerEventData e){
			//タイルの取得
			var tile=GameObject.FindGameObjectsWithTag("Tile")
				.Where(x=>RectTransformUtility.RectangleContainsScreenPoint(x.GetComponent<RectTransform>(),e.position)==true
                    &&x.GetComponent<ETile>().isAttachable==true)
				.Select(x=>x.GetComponent<ETile>())
                .FirstOrDefault();

            //タイル外
			if (tile == null) {
                                rectTransform.position = oldPosition;
                return;
			}
                        //他キャラクターの取得
                        var otherTile=GameObject.FindGameObjectsWithTag("Edit/Character")
                .Where(x=>x.GetComponent<ECharacterIcon>().vect2D==tile.vect)
                .FirstOrDefault();

            //既にいる
            if(otherTile!=null){
                rectTransform.position = oldPosition;
                return;
            }
           

            rectTransform.position = CSTransform.CopyVector3(tile.GetComponent<RectTransform>().position);
            oldPosition = CSTransform.CopyVector3(rectTransform.position);
            vect2D = tile.vect;
            

			
		}
    }




}
