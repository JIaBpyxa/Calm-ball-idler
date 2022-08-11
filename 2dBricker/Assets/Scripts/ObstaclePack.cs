using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class ObstaclePack : MonoBehaviour
    {
        public void Move(Vector3 fromPosition, Vector3 toPosition, float duration)
        {
            transform.position = fromPosition;
            transform.DOMove(toPosition, duration).SetEase(Ease.InOutSine).onComplete += () =>
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            };
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, ObstaclePack>
        {
        }
    }
}