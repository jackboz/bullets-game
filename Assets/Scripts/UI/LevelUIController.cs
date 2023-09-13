using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUIModule.UI.StateMachine;
using MyGame;

namespace MyUIModule.UI
{
    /*
     * All level logic should be implemented in LevelManager
     */
    public class LevelUIController : MonoBehaviour
    {
        UIStateMachineController stateMachineController = new UIStateMachineController();
        LevelManager levelManager;

        void Start()
        {
            IUIState[] states = GetComponents<IUIState>();
            // This initialization will trigger OnEnterUIState method of the Start (0) state
            stateMachineController.Init(states);
            if (states.Length == 0)
            {
                Debug.LogError("No UI states attached to LevelUIController game object");
            }
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("LevelManager not found");
            }
        }

        public void StartButtonClick()
        {
            stateMachineController.SwitchUI(UIStates.Level);
        }

        public void NextButtonClick()
        {
            levelManager.NextLevel();
        }

        public void RestartLevelButtonClick()
        {
            levelManager.Restart();
        }

        public void ActivateWinPanel()
        {
            stateMachineController.SwitchUI(UIStates.Win);
        }

        public void ActivateLossPanel()
        {
            stateMachineController.SwitchUI(UIStates.Loss);
        }

        public void RestartGameButtonClick()
        {
            levelManager.RestartGame();
        }

        public UIStates GetCurrentUIState()
        {
            return stateMachineController.CurrentState;
        }
    }
}