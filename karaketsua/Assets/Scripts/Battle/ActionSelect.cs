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
using System;

//UIの設定
public class ActionSelect : Singleton<ActionSelect>
{
    public enum UIState
    {
        Awake,
        StartWave,
        Active,
        EndActive,
        SelectAttack,
        ExecuteAttack,
        Cancel,
        SelectDeffence,
        ExecuteDeffence,
        SelectWaza,
        SelectSpecialAttack,
        CameraMove,
        CameraEvent,
        CharacterMove

    }
    //キャラクター行動選択時
    public event Action OnAwakeE;
    public event Action OnStartWaveE;
    public event Action OnActiveE;
    public event Action OnEndActiveE;
    public event Action OnSelectAttackE;
    public event Action OnExecuteAttackE;
    public event Action OnCancelE;
    public event Action OnSelectDeffenceE;
    public event Action OnExecuteDeffenceE;
    public event Action OnSelectWazaE;
    public event Action OnSelectSpecialAttackE;
    public event Action OnCameraMoveE;
    public event Action OnCameraEventE;
    public event Action OnCharacterMoveE;

    public UIState nowUIstate = UIState.Awake;
    public void UpdateActionState(UIState _UIstate)
    {
        nowUIstate = _UIstate;
        switch (nowUIstate)
        {
            //ゲーム起動
            case UIState.Awake:
                OnAwakeE();
                break;
            //ゲーム始動
            case UIState.StartWave:
                OnStartWaveE();
                break;
            //キャラクターアクティブ
            case UIState.Active:
                OnActiveE();
                break;
            //キャラクター行動終了
            //ActiveTimeの再稼働
            case UIState.EndActive:
                OnEndActiveE();
                break;
            //攻撃ボタン
            case UIState.SelectAttack:
                OnSelectAttackE();
                break;
            //攻撃実行
            case UIState.ExecuteAttack:
                OnExecuteAttackE();
                break;
            //戻るボタン
            case UIState.Cancel:
                OnCancelE();
                break;
            //防御ボタン
            case UIState.SelectDeffence:
                OnSelectDeffenceE();
                break;
            //防御実行
            case UIState.ExecuteDeffence:
                OnExecuteDeffenceE();
                break;
            //技選択ボタン
            case UIState.SelectWaza:
                OnSelectWazaE();
                break;
            //特殊攻撃ボタン
            case UIState.SelectSpecialAttack:
                OnSelectSpecialAttackE();
                break;
            //カメラ移動
            case UIState.CameraMove:
                OnCameraMoveE();
                break;
                //カメラ
            case UIState.CameraEvent:
                OnCameraEventE();
                break;
            case UIState.CharacterMove:
                OnCharacterMoveE();
                break;

                



        }
    }

    //宣言
    public Character activeCharacter;

    void Start()
    {
        //戦闘開始
        //actionUI.OnStartBattleScene();
        //bottomControllerUI.OnStartBattleScene();

        UpdateActionState(UIState.Awake);
        UpdateActionState(UIState.StartWave);
    }

    #region::他のスクリプトから呼ぶ

    //ボタンを表示
    public void OnActiveCharacter(Character activeChara)
    {
        activeCharacter = activeChara;
        UpdateActionState(UIState.Active);

    }
    public void EndActiveAction()
    {
        UpdateActionState(UIState.EndActive);
        activeCharacter = null;
    }


    #endregion::他のスクリプトから呼ぶ




    #region::UIから使用


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