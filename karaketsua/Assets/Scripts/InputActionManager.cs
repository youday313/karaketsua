//InputActionManager
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputActionManager : Singleton<InputActionManager>
{
	//public

	//private

    public CommandState commandState = CommandState.None;

	void Start ()
	{
        
	}

    //void OnEnable()
    //{
    //    IT_Gesture.onDraggingEndE += OnDragEnd;
    //}
    //void OnDisable()
    //{
    //    IT_Gesture.onDraggingEndE -= OnDragEnd;
    //}
    //void Update ()
    //{
		
    //}

    //void OnDragEnd(DragInfo dragInfo)
    //{
    //    if (commandState != CommandState.None) return;

    //    if (commandState == CommandState.Move)
    //    {
    //        //PlayerOwner.Instance.MoveActiveCharacter(dragInfo);
    //    }





    //}
}