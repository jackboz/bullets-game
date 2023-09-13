using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUIModule.UI.StateMachine
{
    public interface IUIState
    {
        public void OnEnterUIState();
        public void OnExitUIState();
    }

    public enum UIStates
    {
        Start,
        Level,
        Win,
        Loss
    }

    public class UIStateMachineController
    {
        Dictionary<UIStates, IUIState> _states = new Dictionary<UIStates, IUIState>();
        public UIStates CurrentState { get; private set; }

        public void Init(IUIState[] uistates)
        {
            if (uistates.Length == 0)
            {
                Debug.LogError("UI states are empty");
            }
            for (int i = 0; i < uistates.Length; i++)
            {
                _states.Add((UIStates)i, uistates[i]);
            }
            CurrentState = 0;
            _states[CurrentState].OnEnterUIState();
        }

        public void SwitchUI(UIStates newState)
        {
            _states[CurrentState].OnExitUIState();
            CurrentState = newState;
            _states[CurrentState].OnEnterUIState();
        }
    }
}
