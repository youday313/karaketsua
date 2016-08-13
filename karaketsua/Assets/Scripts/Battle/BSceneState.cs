using UnityEngine;
using System.Collections;
using System;

using BattleScene;

namespace BattleScene
{

    public class BSceneState : SingletonMonoBehaviour<BSceneState>
    {

        public enum State
        {
            StartWave,
            UnSelect,
            Selectable,
            UnSelectable,
            EndWave
        }
        public event Action UpdateStateE;
        public event Action StartWave;

        public State nowState = State.StartWave;
        public void UpdateState(State _state)
        {
            nowState = _state;
            UpdateStateE();
        }

        void Update()
        {
            if (nowState == State.StartWave)
            {
                StartWave();
                nowState = State.UnSelect;
            }
        }
    }
}