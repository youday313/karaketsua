using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EditScene{

	public class ECharacterIcon : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler {
		

	// Use this for initialization
	void Start () {


			oldPosition = CSTransform.CopyVector3(GetComponent<RectTransform> ().position);
		}
	
	// Update is called once per frame
	void Update () {
	
	}
		public Vector3 oldPosition;
		RectTransform rectTransform;

		public void OnBeginDrag(PointerEventData e){
			rectTransform.position = e.position;
			//obj.SetAsFirstSibling();
		}
		public void OnDrag(PointerEventData e){
			rectTransform.position = e.position;
		}
		public void OnEndDrag(PointerEventData e){
			//タイルの取得
			var tile=GameObject.FindGameObjectsWithTag("Tile")
				.Where(x=>x.GetComponent<RectTransform>().rect.Contains(e.position)==true)
				.FirstOrDefault();

			if (tile == null) {
				
			}

			//obj.SetAsLastSibling();
		}
	}




}
	