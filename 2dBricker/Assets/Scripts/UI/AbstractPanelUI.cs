using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.UI
{
    public abstract class AbstractPanelUI : MonoBehaviour
    {
        public virtual void OpenPanel()
        {
            OpeningAnimation(.3f);
        }

        public virtual void ClosePanel(Action onClosedAction = null)
        {
            const float duration = .3f;
            ClosingAnimation(duration);

            if (onClosedAction == null) return;
            var timerObservable = UniRx.Observable.Timer(TimeSpan.FromSeconds(duration));
            ObservableExtensions.Subscribe(timerObservable, _ => onClosedAction());
        }

        protected virtual void OpeningAnimation(float duration)
        {
            transform.localPosition = Vector3.up * 3000;
            transform.DOLocalMoveY(0f, duration).SetEase(Ease.InOutExpo);
        }

        protected virtual void ClosingAnimation(float duration)
        {
            transform.DOLocalMoveY(-3000f, duration).SetEase(Ease.InOutExpo);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, AbstractPanelUI>
        {
        }
    }
}