using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;
using TMPro;

namespace MyUIModule
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textLabel;
        [SerializeField] float initialValue = 0;
        Slider slider;
        Image fillArea;

        void OnEnable()
        {
            Actions.OnEnemyKilled += UpdateProgress;
        }

        void OnDisable()
        {
            Actions.OnEnemyKilled -= UpdateProgress;
        }

        void Awake()
        {
            slider = gameObject.GetComponent<Slider>();
            fillArea = transform.Find("Fill Area/Fill").GetComponent<Image>();
            if (fillArea == null)
            {
                Debug.LogError("Cound't find \"Fill Area/Fill\" Image component");
            }
            SetProgress(initialValue);
        }

        public void SetProgress(float newProgress)
        {
            slider.value = Mathf.Clamp01(newProgress);
            _textLabel.SetText(newProgress.ToString("P0"));
        }

        public void ChangeColor(string color)
        {
            Color newCol;
            if (ColorUtility.TryParseHtmlString(color, out newCol))
            {
                fillArea.color = newCol;
            }
        }

        private void UpdateProgress(Enemy enemy)
        {
            SetProgress(GameProgressStatic.LevelProgress);
        }
    }
}