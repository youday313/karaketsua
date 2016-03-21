using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FieldScene;

namespace FieldScene{

public class FCharacter : MonoBehaviour {


		float moveSpeed = 2.5f;
		Animator animator;
		Vector2 dragPosition;
		bool isMovIng;
	// Use this for initialization
	void Start () {
			
			animator = GetComponent<Animator> ();

			IT_Gesture.onDraggingStartE += OnDraggingStart;

	}
	
	// Update is called once per frame
	void Update () {

			SetMoveAnimation (isMovIng);
			isMovIng = false;
	}
		//移動開始
		void OnDraggingStart(DragInfo drag){
			//タッチ位置取得

			dragPosition = drag.pos;
			IT_Gesture.onMouse1E += OnTouch;

			IT_Gesture.onDraggingEndE += OnDraggingEnd;

		}
			
		//移動中
		void OnTouch(Vector2 touch){
			
			var delta = touch - dragPosition;
			isMovIng = delta.magnitude > 1;
			var speed = delta.normalized * moveSpeed * Time.deltaTime;
			var newPos = new Vector3 (speed.x,0,speed.y);
			transform.position+=newPos;
		}
			
		void OnDraggingEnd(DragInfo drag){
			
			IT_Gesture.onMouse1E -= OnTouch;

			IT_Gesture.onDraggingEndE -= OnDraggingEnd;
		}

		void SetMoveAnimation(bool isTrue){
			animator.SetBool("Walk",isTrue);
		}




}
}