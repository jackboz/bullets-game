using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

namespace MyUIModule.UI.StateMachine
{
    public class LostLevelState : MonoBehaviour, IUIState
    {
        [SerializeField] GameObject panel;
        [SerializeField] LevelManager levelManager;

        void Start()
        {
            if (panel == null)
            {
                Debug.LogError("UI Panel is not set");
            }
            if (levelManager == null)
            {
                Debug.LogError("Level Manager is not set");
            }
        }

        public void OnEnterUIState()
        {
            panel.SetActive(true);
        }

        public void OnExitUIState()
        {
            panel.SetActive(false);
            levelManager.Restart();
        }
    }
}