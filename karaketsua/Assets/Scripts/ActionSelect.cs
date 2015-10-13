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
    public GameObject cancel;
	//private

	void Start ()
	{
        commands.SetActive(false);
        swipeIcon.SetActive(false);
        cancel.SetActive(false);
    }
	
	void Update ()
	{
		
	}

    //ボタンを表示
    public void SetActiveAction()
    {
        commands.SetActive(true);
    }

    //ボタンから使用
    public void SelectCommandFromButton(string state)
    {
        var command = EnumRapper.GetEnum<CommandButton>(state);
        swipeIcon.SetActive(true);

        if (command == CommandButton.Attack)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.TargetSelect);
            cancel.SetActive(true);
        }
        else if (command == CommandButton.Skill)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.Skill);

        }
        else if (command == CommandButton.Wait)
        {
            PlayerOwner.Instance.SetCommandState(CommandState.Wait);
        }
        commands.SetActive(false);
    }
    public void CancelButton()
    {
        cancel.SetActive(false);

        SetActiveAction();
    }
}