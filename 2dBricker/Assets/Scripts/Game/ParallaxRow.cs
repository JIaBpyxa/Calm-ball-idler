using DG.Tweening;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class ParallaxRow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space] [SerializeField] private float _meanMoveDuration = 90f;

        [Space] [SerializeField] private float _leftEnd;
        [SerializeField] private float rightEnd;


        private void Awake()
        {
            StartMoving();
        }

        private void StartMoving()
        {
            var isLeftToRight = Random.value > .5f;
            var startPoint = isLeftToRight ? _leftEnd : rightEnd;
            var endPoint = isLeftToRight ? rightEnd : _leftEnd;

            var moveDuration = _meanMoveDuration * Random.Range(.75f, 1.25f);

            var sequence = DOTween.Sequence();
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(startPoint, .05f));
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(endPoint, moveDuration));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }
    }
}