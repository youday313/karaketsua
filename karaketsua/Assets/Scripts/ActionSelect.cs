//ActionSelect
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CommandButton { Attack,Skill,Wait};

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
        var command = EnumRapper.GetEnum<CommandButton>(state);
        swipeIcon.SetActive(true);

        if (command == CommandButton.Attack)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.TargetSelect);

        }
        else if (command == CommandButton.Skill)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.Skill);

        }
        else if (command == CommandButton.Wait)
        {

        }
        commands.SetActive(false);
    }
}