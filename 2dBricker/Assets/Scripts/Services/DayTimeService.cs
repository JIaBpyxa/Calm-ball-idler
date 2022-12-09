using UniRx;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class DayTimeService : MonoBehaviour
    {
        public ReactiveProperty<DayTime> CurrentDayTime = new(DayTime.Day);

        public void SwitchDayTime()
        {
            if (CurrentDayTime.Value == DayTime.Day)
            {
                CurrentDayTime.Value = DayTime.Night;
            }
            else
            {
                CurrentDayTime.Value = DayTime.Day;
            }
        }

        public enum DayTime
        {
            Day,
            Night
        }
    }
}