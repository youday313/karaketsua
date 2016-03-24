using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FieldScene;

namespace FieldScene
{

    public class FCharacter : MonoBehaviour
    {

        CharacterController controller;

        float moveSpeed = 2.5f;
        Animator animator;
        Vector2 dragPosition;
        bool isMovIng;
        // Use this for initialization
        void Start()
        {

            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            IT_Gesture.onDraggingStartE += OnDraggingStart;

        }

        // Update is called once per frame
        void Update()
        {

            SetMoveAnimation(isMovIng);
            isMovIng = false;
        }
        //移動開始
        void OnDraggingStart(DragInfo drag)
        {
            //タッチ位置取得

            dragPosition = drag.pos;
            IT_Gesture.onMouse1E += OnTouch;

            IT_Gesture.onDraggingEndE += OnDraggingEnd;

        }

        //移動中
        void OnTouch(Vector2 touch)
        {

            var delta = touch - dragPosition;
            isMovIng = delta.magnitude > 1;
            if (isMovIng == false)
            {
                return;
            }
            var speed = delta.normalized * moveSpeed * Time.deltaTime;
            var newPos = new Vector3(speed.x, 0, speed.y);
            //transform.position += newPos;
            controller.Move(newPos);

            transform.rotation = Quaternion.LookRotation(newPos);


        }

        void OnDraggingEnd(DragInfo drag)
        {

            IT_Gesture.onMouse1E -= OnTouch;

            IT_Gesture.onDraggingEndE -= OnDraggingEnd;
        }

        void SetMoveAnimation(bool isTrue)
        {
            animator.SetBool("Walk", isTrue);
        }

        //Vector3 oldPosition;
        //void OnTriggerEnter(Collider coll)
        //{
        //    if (coll.tag == "Field/Wall")
        //    {
        //        transform.position = oldPosition;
        //    }
        //}
        //void LateUpdate()
        //{
        //    oldPosition = CSTransform.CopyVector3(transform.position);
        //}






    }
}