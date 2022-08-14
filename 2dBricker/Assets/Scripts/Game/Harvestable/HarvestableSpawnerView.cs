using DG.Tweening;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class HarvestableSpawnerView : MonoBehaviour
    {
        [SerializeField] private Transform _lockerHolder;

        public void Lock()
        {
            _lockerHolder.localPosition = Vector3.up * 1000f;
            _lockerHolder.gameObject.SetActive(true);
            _lockerHolder.DOLocalMoveY(0f, .5f);
        }

        public void Unlock()
        {
            _lockerHolder.DOLocalMoveY(1000f, .5f).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                _lockerHolder.gameObject.SetActive(false);
            };
        }
    }
}