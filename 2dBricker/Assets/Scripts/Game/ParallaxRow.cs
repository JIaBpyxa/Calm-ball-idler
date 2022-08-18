using DG.Tweening;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class ParallaxRow : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _festivalColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space] [SerializeField] private float _meanMoveDuration = 90f;

        [Space] [SerializeField] private float _leftEnd;
        [SerializeField] private float rightEnd;

        [Inject]
        private void Construct(ScoreModifierService scoreModifierService)
        {
            scoreModifierService.IsActive.Subscribe(HandleScoreModifierActivation);
        }

        private void Awake()
        {
            StartMoving();
            EndFestival();
        }

        private void StartMoving()
        {
            var isLeftToRight = Random.value > .5f;
            var startPoint = isLeftToRight ? _leftEnd : rightEnd;
            var endPoint = isLeftToRight ? rightEnd : _leftEnd;

            var moveDuration = _meanMoveDuration * Random.Range(.75f, 1.25f);

            var sequence = DOTween.Sequence();
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(startPoint, .01f).SetEase(Ease.Linear));
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(endPoint, moveDuration).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }

        private void HandleScoreModifierActivation(bool isActive)
        {
            if (isActive)
            {
                StartFestival();
            }
            else
            {
                EndFestival();
            }
        }

        private void StartFestival()
        {
            _spriteRenderer.DOColor(_festivalColor, 1f);
        }

        private void EndFestival()
        {
            _spriteRenderer.DOColor(_defaultColor, 1f);
        }
    }
}