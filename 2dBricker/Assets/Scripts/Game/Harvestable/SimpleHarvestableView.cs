using System;
using DG.Tweening;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class SimpleHarvestableView : AbstractHarvestableView
    {
        public override void Activate()
        {
            transform.DOKill();
            transform.localScale = Vector3.zero;
            transform.DOScale(initialScale, activationDuration).SetEase(Ease.InOutCirc);
        }

        public override void Deactivate(Action deactivationAction)
        {
            //harvestedParticles.Play();
            transform.DOKill();

            transform.DOScale(Vector3.zero, deactivationDuration).SetEase(Ease.InOutCirc).onComplete +=
                () => deactivationAction?.Invoke();
        }
    }
}