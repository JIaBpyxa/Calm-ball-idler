using System;
using System.Numerics;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.Service
{
    public class ScoreService
    {
        public readonly ReactiveProperty<BigInteger> Score;

        private const int SavePeriod = 5;

        public ScoreService()
        {
            Score = new ReactiveProperty<BigInteger>(SaveService.GetScore());

            var saveObservable = Observable.Interval(TimeSpan.FromSeconds(SavePeriod));
            ObservableExtensions.Subscribe(saveObservable, _ => SaveService.SaveScore(Score.Value));
        }

        public BigInteger GetScore() => Score.Value;

        public void AddScore(BigInteger addedScore)
        {
            UpdateScore(BigInteger.Add(Score.Value, addedScore));
        }

<<<<<<< HEAD
        public void ReduceScore(BigInteger price)
        {
            if (!IsPurchaseAvailable(price)) return;

            Score.Value -= price;
=======
        public bool ReduceScore(BigInteger price)
        {
            if (!IsPurchaseAvailable(price)) return false;

            Score.Value -= price;
            return true;
>>>>>>> develop
        }

        public void UpdateScore(BigInteger newScore)
        {
            Score.Value = newScore;
        }

        public bool IsPurchaseAvailable(BigInteger price)
        {
            return price <= Score.Value;
        }
        
        public static string GetStringFromValue(BigInteger value)
        {
            var s = value.ToString();
            var length = s.Length;

            switch (length)
            {
                case < 4:
                    return s;
                case < 7:
                    return $"{s[..(length - 3)]}.{s.Substring(length - 3, 1)}K";
                case < 10:
                    return $"{s[..(length - 6)]}.{s.Substring(length - 6, 1)}M";
                case < 13:
                    return $"{s[..(length - 9)]}.{s.Substring(length - 9, 1)}B";
                case < 16:
                    return $"{s[..(length - 12)]}.{s.Substring(length - 12, 1)}T";
                default:
                {
                    var symbol = (char)('a' + (length - 16) / 3);
                    return $"{s[..(length - (length - 1) / 3 * 3)]}.{s.Substring(length - (length - 1) / 3 * 3, 1)}{symbol}{symbol}";
                }
            }
        }
    }
}