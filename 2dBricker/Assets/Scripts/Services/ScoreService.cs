using System;
using System.Numerics;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.Service
{
    public class ScoreService
    {
        public ReactiveProperty<BigInteger> Score;

        private const int SavePeriod = 5;

        public ScoreService()
        {
            Score = new ReactiveProperty<BigInteger>(SaveService.GetScore());

            var saveObservable = Observable.Interval(TimeSpan.FromSeconds(SavePeriod));
            ObservableExtensions.Subscribe(saveObservable, _ => SaveService.SaveScore(Score.Value));
        }

        public BigInteger GetScore() => Score.Value;

        public void AddScore(int addedScore)
        {
            UpdateScore(BigInteger.Add(Score.Value, addedScore));
        }

        public void ReduceScore(BigInteger price)
        {
            if (!IsPurchaseAvailable(price)) return;

            Score.Value -= price;
        }

        public void UpdateScore(BigInteger newScore)
        {
            Score.Value = newScore;
        }

        public bool IsPurchaseAvailable(BigInteger price)
        {
            return price <= Score.Value;
        }
    }
}