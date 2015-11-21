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
	//private

    Character activeCharacter;

	void Start ()
	{
        commands.SetActive(false);
    }
	
	void Update ()
	{
		
	}

    //ボタンを表示
    public void SetActiveAction(Character activeChara)
    {
        activeCharacter = activeChara;
        commands.SetActive(true);
    }

    //ボタンから使用

    public void OnAttackButton()
    {
        activeCharacter.SetAttackMode();
        commands.SetActive(false);
    }
    public void OnSkillButton()
    {
        activeCharacter.SetSkillMode();
        commands.SetActive(false);
    }
    public void OnWaitButton()
    {
        activeCharacter.SetWaitMode();
        commands.SetActive(false);
    }

    public void OnAndoButton()
    {
        activeCharacter.SetAndo();
        commands.SetActive(true);

        //SetActiveAction();
    }

}