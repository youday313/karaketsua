//PlayerOwner
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CommandState { None, Moved, TargetSelect, Attack, Skill,Wait, End };

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
        commandState = CommandState.None;

    }
    public void OnEndActive()
    {
        activeCharacter = null;
        SetCommandState(CommandState.End);
        cameraMove.MoveToLean();
        WaitTimeManager.Instance.RestartWaitTime();

    }

    void OnEnable()
    {
        IT_Gesture.onDraggingEndE += OnDragEnd;
        //IT_Gesture.onTouchDownE += OnTouch;
        IT_Gesture.onShortTapE += OnShortTap;
    }
    void OnDisable()
    {
        IT_Gesture.onDraggingEndE -= OnDragEnd;
        //IT_Gesture.onTouchDownE -= OnTouch;
        IT_Gesture.onShortTapE -= OnShortTap;
    }


    void OnDragEnd(DragInfo dragInfo)
    {
        //if (commandState == CommandState.None) return;

        if (commandState == CommandState.None)
        {
            MoveActiveCharacter(dragInfo.delta);
        }

        //commandState = CommandState.None;
    }






    void OnShortTap(Vector2 touchInfo)
    {
        Debug.Log("set");
        //攻撃先選択
        if (commandState == CommandState.TargetSelect)
        {
            activeCharacter.SetTarget(touchInfo);
        }
        else if (commandState == CommandState.Attack)
        {
            activeCharacter.Attack(touchInfo);
        }


    }
    void OnSwiping(SwipeInfo swipeInfo)
    {
        activeCharacter.SkillSwipe(swipeInfo);
    }

    void OnTouchUp(Vector2 touch)
    {
        if (commandState == CommandState.Skill)
        {
            activeCharacter.SetSkillMode();
        }
    }

    public void SetCommandState(CommandState state)
    {
        commandState = state;
        //ターゲット選択なら上下左右のタイル色変更
        if (commandState==CommandState.TargetSelect) {
            activeCharacter.SetAttackMode();
            //activeCharacter.ChangeNeighborTile(TileState.Attack);
            //BattleStage.Instance.ChangeNeighborTilesColor(activeCharacter.positionArray, TileState.Attack);
        }
        else if(commandState==CommandState.Skill){
            activeCharacter.SetSkillMode();
            IT_Gesture.onSwipingE += OnSwiping;
            //IT_Gesture.onMouse1UpE += OnTouchUp;
        }
        else if (commandState==CommandState.Wait)
        {
            activeCharacter.Wait();
        }
        else if (commandState == CommandState.End)
        {
            IT_Gesture.onSwipingE -= OnSwiping;
            //IT_Gesture.onMouse1UpE -= OnTouchUp;
        }
    }


    void MoveActiveCharacter(Vector2 delta)
    {
        //どの方向に動くか
        //x方向
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            activeCharacter.Move(new IntVect2D((int)Mathf.Sign(delta.x), 0));

        }
            //y方向
        else
        {
            activeCharacter.Move(new IntVect2D(0, (int)Mathf.Sign(delta.y)));
        }
    }


}