using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

namespace MyUIModule.UI.StateMachine
{
    public class WinLevelState : MonoBehaviour, IUIState
    {
        [SerializeField] GameObject panel;
        [SerializeField] LevelManager levelManager;
        [SerializeField] GameObject winPanel;
        [SerializeField] GameObject endPanel;

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
            if (GameProgressStatic.LevelNumber == 3)
            {
                winPanel.SetActive(false);
                endPanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(true);
                endPanel.SetActive(false);
            }
        }

        public void OnExitUIState()
        {
            panel.SetActive(false);
            //levelManager.Restart();
        }
    }
}