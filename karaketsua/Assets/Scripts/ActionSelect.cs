//ActionSelect
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CommandState { None,Move, Attack, Skill };

public class ActionSelect : Singleton<ActionSelect>
{
	//public
    public GameObject commands;
    public GameObject swipeIcon;
	//private

	void Start ()
	{
        commands.SetActive(false);
        swipeIcon.SetActive(false);
	}
	
	void Update ()
	{
		
	}

    public void SetActiveAction()
    {
        commands.SetActive(true);
    }

    public void SelectCommandFromButton(string state)
    {
        var command=EnumRapper.GetEnum<CommandState>(state);
        swipeIcon.SetActive(true);

        if (command == CommandState.Move)
        {
            PlayerOwner.Instance.commandState = CommandState.Move;
        }
        else if (command == CommandState.Attack)
        {
            PlayerOwner.Instance.commandState = CommandState.Attack;

        }
        else if (command == CommandState.Skill)
        {
            PlayerOwner.Instance.commandState = CommandState.Skill;

        }
        commands.SetActive(false);
    }
}