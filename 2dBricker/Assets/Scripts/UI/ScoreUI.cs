using TMPro;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Game;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private ScoreService _scoreService;

        [Inject]
        private void Construct(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void Start()
        {
            if (_scoreText is null) return;

            _scoreService.Score.Subscribe(UpdateText);
        }

        private void UpdateText(int score)
        {
            _scoreText.text = $"{score}";
        }
    }
}