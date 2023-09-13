#define TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUIModule.UI;
using StarterAssets;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        LevelUIController _levelUIController;
        [SerializeField]
        ThirdPersonController _thirdPersonController;


        void OnEnable()
        {
            Actions.OnLevelFailed += FailLevel;
            Actions.OnLevelWon += WinLevel;
        }

        void OnDisable()
        {
            Actions.OnLevelFailed -= FailLevel;
            Actions.OnLevelWon -= WinLevel;
        }

        void Awake()
        {
            if (_levelUIController == null)
            {
                Debug.LogError("Set level ui controller");
                return;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (_levelUIController.GetCurrentUIState() == MyUIModule.UI.StateMachine.UIStates.Start)
                {
                    _levelUIController.StartButtonClick();
                }
            }
        }

        public void StartMenu()
        {
            _thirdPersonController.TurnOff();
            Time.timeScale = 0f;
        }

        public void StartLevel()
        {
            _thirdPersonController.TurnOn();
            Time.timeScale = 1f;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        public void FailLevel()
        {
            _thirdPersonController.TurnOff();
            Time.timeScale = 0f;
            _levelUIController.ActivateLossPanel();
        }

        public void WinLevel()
        {
            _thirdPersonController.TurnOff();
            Time.timeScale = 0f;
            _levelUIController.ActivateWinPanel();
        }

        public void NextLevel()
        {
            GameProgressStatic.LevelNumber += 1;
            Restart();
        }

        public void RestartGame()
        {
            GameProgressStatic.LevelNumber = 1;
            Restart();
        }
    }
}
