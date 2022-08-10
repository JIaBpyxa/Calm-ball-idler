using System;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.Game
{
    public class ScoreService
    {
        public IntReactiveProperty Score;

        private const int SavePeriod = 5;

        public ScoreService()
        {
            Score = new IntReactiveProperty(SaveService.GetScore());

            var saveObservable = Observable.Interval(TimeSpan.FromSeconds(SavePeriod));
            ObservableExtensions.Subscribe(saveObservable, _ => SaveService.SaveScore(Score.Value));
        }

        public int GetScore() => Score.Value;

        public void AddScore(int addedScore)
        {
            UpdateScore(Score.Value + addedScore);
        }

        public void UpdateScore(int newScore)
        {
            Score.Value = newScore;
        }
    }
}