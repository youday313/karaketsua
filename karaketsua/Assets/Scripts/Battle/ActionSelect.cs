//ActionSelect
//作成日
//<summary>
//
//</summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum CommandButton { Attack, Skill, Wait };

//UIの設定
public class ActionSelect : Singleton<ActionSelect>
{

    //宣言
    
    public UIActionSelect actionUI;
    public UIBottomController bottomControllerUI;
    //public UIWaza wazaUI;
    //public UIActionExecute executeActionUI;

    Character activeCharacter;

    void Start()
    {
        //戦闘開始
        actionUI.OnStartBattleScene();
        bottomControllerUI.OnStartBattleScene();
    }

    #region::他のスクリプトから呼ぶ

    //ボタンを表示
    public void OnActiveCharacter(Character activeChara)
    {
        activeCharacter = activeChara;
        actionUI.OnActiveCharacter();
        bottomControllerUI.OnActiveCharacter();
    }
    public void EndActiveAction()
    {
        bottomControllerUI.UnSetActive();
        activeCharacter = null;
    }

    //カメラが移動
    public void OnCameraMove()
    {
        bottomControllerUI.OnCameraMove();
    }

    //行動選択時
    //キャンセルを有効化
    public void OnActionSelect()
    {
        bottomControllerUI.OnActionSelect();
    }


    #endregion::他のスクリプトから呼ぶ




    #region::UIから使用

    #region::ActionUI
    //攻撃ボタン
    public void OnAttackButton()
    {
    }
    
    //防御ボタン
    public void OnDeffenceButton()
    {

    }

    #endregion::ActionUI

    #region::BottomUI
    //戻るボタン：行動キャンセル
    public void OnCancelButton()
    {
        //activeCharacter.SetAndo();
        //commands.SetActive(true);
        //andoButton.SetActive(false);

    }
    //カメラボタン
    //カメラ操作時：カメラリセット、カメラ非操作時：カメラ切り替え
    public void OnCameraButton()
    {

    }


    #endregion::BottomUI
    
    #region::WazaUI
    //通常技
    public void OnWazaButton(int number)
    {
        //キャラクターの攻撃の設定
        //activeCharacter.setAttack();

    }
    //移動技
    public void OnSpecialWazaButton()
    {
        //キャラクターの攻撃の設定
    }

    #endregion::WazaUI

    #region::ExecuteUI
    //攻撃決定
    public void OnExecuteAttackButton()
    {
        activeCharacter.ExecuteAttack();
    }

    //防御決定
    public void OnExecuteDeffenceButton()
    {
        activeCharacter.SetWaitMode();
        EndActiveAction();
    }

    #endregion::ExecuteUI

    #endregion::UIから使用

    public void EnableAttackButton()
    {
        //decideAttackButton.SetActive(true);
    }
    //moveAttack
    public void EnableMoveAttackButton()
    {
        //decideMoveAttackButton.SetActive(true);
    }
    public void DisableMoveAttackButton()
    {
        //decideMoveAttackButton.SetActive(false);
    }

    public void OnExecuteMoveAttackButton()
    {
        activeCharacter.ExcuteMoveAttack();
    }
}