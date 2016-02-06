using UnityEngine;
using System.Collections;
using System;

public class BSceneState : Singleton<BSceneState> {

    public enum State
    {
        StartWave,
        SelectAttack,//攻撃選択
        SelectDeffence,//防御選択
        SelectWaza,//技選択
        

        Attacking,//攻撃開始

    }
    //キャラクター行動選択時
    public event Action UpdateStateE;


    public State nowstate = State.StartWave;
    public void UpdateState(State _state)
    {
        nowstate = _state;
        UpdateStateE();
    }
}
