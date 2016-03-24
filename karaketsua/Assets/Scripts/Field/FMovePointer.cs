using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

using FieldScene;

namespace FieldScene
{
    public class FMovePointer : MonoBehaviour
    {
        RectTransform rectTransform;
        Vector2 DragPosition
        {
            get { return dragPosition; }
            set { dragPosition = value;
            UpdatePosition();
            }
        }
        Vector2 dragPosition;
        Image pointer;
        
        // Use this for initialization
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            pointer = GetComponent<Image>();
            pointer.enabled = false;

            IT_Gesture.onDraggingStartE += OnDraggingStart;
        }

        // Update is called once per frame
        void Update()
        {
        }
        //移動開始
        void OnDraggingStart(DragInfo drag)
        {
            //タッチ位置取得
            pointer.enabled = true;
            DragPosition = drag.pos;
            IT_Gesture.onMouse1E += OnTouch;

            IT_Gesture.onDraggingEndE += OnDraggingEnd;

        }
        //移動中
        void OnTouch(Vector2 touch)
        {
            var delta = touch - dragPosition;

            rectTransform.localEulerAngles = new Vector3(0,0, CalcRadian(dragPosition, touch));


        }

        void OnDraggingEnd(DragInfo drag)
        {
            pointer.enabled = false;

            IT_Gesture.onMouse1E -= OnTouch;

            IT_Gesture.onDraggingEndE -= OnDraggingEnd;
        }

        void UpdatePosition()
        {
            rectTransform.position = DragPosition;
        }
        //2点間の角度を求める
        private float CalcRadian(Vector2 from, Vector2 to)
        {
            float dx = to.x - from.x;
            float dy = to.y - from.y;
            float radian = Mathf.Atan2(dy, dx);
            return radian*180/Mathf.PI-90;
        }

        public void OnDisable()
        {
            IT_Gesture.onDraggingStartE -= OnDraggingStart;
            IT_Gesture.onMouse1E -= OnTouch;
            IT_Gesture.onDraggingEndE -= OnDraggingEnd;
        }

    }

}