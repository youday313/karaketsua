//PlayerOwner
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerOwner : Singleton<PlayerOwner>
{
	//public

	//private
    CameraMove cameraMove;
    Character activeCharacter;
    public CommandState commandState = CommandState.None;

	void Start ()
	{
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
	}
	
	void Update ()
	{
		
	}


    public void OnActiveCharacter(Character chara)
    {
        activeCharacter = chara;
        cameraMove.MoveToBack();
        ActionSelect.Instance.SetActiveAction();

    }

    void OnEnable()
    {
        Debug.Log("In");
        IT_Gesture.onDraggingEndE += OnDragEnd;
    }
    void OnDisable()
    {
        IT_Gesture.onDraggingEndE -= OnDragEnd;
    }


    void OnDragEnd(DragInfo dragInfo)
    {
        Debug.Log("InEnd");
        if (commandState == CommandState.None) return;

        if (commandState == CommandState.Wait)
        {
            MoveActiveCharacter(dragInfo.delta);
        }
        else if (commandState == CommandState.Attack)
        {
            AttackActiveCharacter(dragInfo.delta);
        }

        commandState = CommandState.None;
    }

    void MoveActiveCharacter(Vector2 delta)
    {


        //どの方向に動くか
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            activeCharacter.Move(new Vect2D<int>((int)Mathf.Sign(delta.x),0));
        }
            //y方向
        else
        {
            activeCharacter.Move(new Vect2D<int>(0, (int)Mathf.Sign(delta.y)));
        }
    }
    void AttackActiveCharacter(Vector2 delta)
    {
        //どの方向に動くか
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            activeCharacter.Attack(new Vect2D<int>((int)Mathf.Sign(delta.x), 0));
        }
        //y方向
        else
        {
            activeCharacter.Attack(new Vect2D<int>(0, (int)Mathf.Sign(delta.y)));
        }
    }

}