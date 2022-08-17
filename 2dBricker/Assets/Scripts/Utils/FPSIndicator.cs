using TMPro;
using UnityEngine;

namespace Vorval.CalmBall.Utils
{
    public class FPSIndicator : MonoBehaviour
    {
        [SerializeField] private Canvas _fpsCanvas;
        [SerializeField] private TextMeshProUGUI _fpsText;
        [SerializeField] private bool _isShow;

        private float _deltaTime;

        private void Start()
        {
            _fpsCanvas.gameObject.SetActive(_isShow);
        }

        private void Update()
        {
            if (!_isShow) return;

            if (Time.timeScale != 0)
            {
                ShowFPS();
            }
        }

        private void ShowFPS()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            _fpsText.text = $"{Mathf.Ceil(fps)}";
        }
    }
}