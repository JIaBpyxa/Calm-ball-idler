using System;
using System.Collections.Generic;
using System.Numerics;
using static Vorval.CalmBall.Game.HarvestableData;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.Service
{
    public class StatsService
    {
        public HarvestableType ChosenHarvestableStats { get; private set; }
        public Action<HarvestableType> OnHarvestableStatsUpdate { get; set; }
        private readonly Dictionary<HarvestableType, HarvestableStatsData> _statsDictionary;

        private const int SavePeriod = 7;

        private StatsService()
        {
            _statsDictionary = new Dictionary<HarvestableType, HarvestableStatsData>
            {
                { HarvestableType.Simple, SaveService.GetHarvestableStats(HarvestableType.Simple) },
                { HarvestableType.Little, SaveService.GetHarvestableStats(HarvestableType.Little) },
                { HarvestableType.Blow, SaveService.GetHarvestableStats(HarvestableType.Blow) },
                { HarvestableType.Slow, SaveService.GetHarvestableStats(HarvestableType.Slow) },
            };

            var saveObservable = Observable.Interval(TimeSpan.FromSeconds(SavePeriod));
            ObservableExtensions.Subscribe(saveObservable, _ =>
            {
                foreach (var statsData in _statsDictionary.Values)
                {
                    SaveService.SaveHarvestableStats(statsData);
                }
            });
        }

        public void ChooseHarvestableStats(HarvestableType harvestableType)
        {
            ChosenHarvestableStats = harvestableType;
        }

        public void AddSpawned(HarvestableType harvestableType)
        {
            var statsData = _statsDictionary[harvestableType];
            statsData.AddSpawned();
            OnHarvestableStatsUpdate?.Invoke(harvestableType);
        }

        public void AddEarnedScore(HarvestableType harvestableType, BigInteger score)
        {
            var statsData = _statsDictionary[harvestableType];
            statsData.AddScoreEarned(score);
            OnHarvestableStatsUpdate?.Invoke(harvestableType);
        }

        public HarvestableStatsData GetHarvestableStatsData(HarvestableType harvestableType)
        {
            return _statsDictionary[harvestableType];
        }
    }

    public class HarvestableStatsData
    {
        public struct RawData
        {
            public int typeId;
            public int spawnedCount;
            public string scoreEarned;
        }

        public HarvestableType Type { get; }
        public int SpawnedCount { get; private set; }
        public BigInteger ScoreEarned { get; private set; }

        public HarvestableStatsData(RawData rawData)
        {
            Type = (HarvestableType)rawData.typeId;
            SpawnedCount = rawData.spawnedCount;

            if (BigInteger.TryParse(rawData.scoreEarned, out var scoreEarned))
            {
                ScoreEarned = scoreEarned;
            }
            else
            {
                ScoreEarned = BigInteger.Zero;
            }
        }

        public void AddSpawned()
        {
            SpawnedCount++;
        }

        public void AddScoreEarned(BigInteger score)
        {
            ScoreEarned += score;
        }

        public RawData GetRawData()
        {
            return new RawData
            {
                typeId = (int)Type,
                spawnedCount = SpawnedCount,
                scoreEarned = ScoreEarned.ToString()
            };
        }
    }
}