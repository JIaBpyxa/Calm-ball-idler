using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Game;

namespace Vorval.CalmBall.Service
{
    public class SaveService
    {
        private const string ScoreKey = "Score";
        private const string HarvestableKey = "Harvestable_";
        private const string HarvestableStatsKey = "Stats_";
        private const string RemoteDataKey = "RemoteData";
        private const string QualityKey = "Quality";
        private const string MusicKey = "Music";
        private const string SfxKey = "Sfx";

        public static void SaveScore(BigInteger score)
        {
            SecurePlayerPrefs.SetString(ScoreKey, score.ToString());
        }

        public static BigInteger GetScore()
        {
            var dataString = SecurePlayerPrefs.GetString(ScoreKey, "0");

            if (BigInteger.TryParse(dataString, out var score))
            {
                return score;
            }

            return BigInteger.Zero;
        }

        public static void SaveHarvestableUpgradeData(HarvestableUpgradeData harvestableUpgradeData)
        {
            var json = JsonUtility.ToJson(harvestableUpgradeData.GetRawData());
            SecurePlayerPrefs.SetString($"{HarvestableKey}{harvestableUpgradeData.Type}", json);
        }

        public static HarvestableUpgradeData GetHarvestableUpgradeData(HarvestableData.HarvestableType harvestableType)
        {
            var dataKey = $"{HarvestableKey}{harvestableType}";
            var rawData = new HarvestableUpgradeData.RawData();

            if (SecurePlayerPrefs.HasKey(dataKey))
            {
                var json = SecurePlayerPrefs.GetString(dataKey);
                rawData = JsonUtility.FromJson<HarvestableUpgradeData.RawData>(json);
            }
            else
            {
                rawData.typeId = (int)harvestableType;
                rawData.isBought = false;
                rawData.powerUpgradeLevel = 0;
                rawData.respawnIntervalUpgradeLevel = 0;
            }

            return new HarvestableUpgradeData(rawData);
        }

        public static void SaveHarvestableStats(HarvestableStatsData harvestableStatsData)
        {
            var json = JsonUtility.ToJson(harvestableStatsData.GetRawData());
            SecurePlayerPrefs.SetString($"{HarvestableStatsKey}{harvestableStatsData.Type}", json);
        }

        public static HarvestableStatsData GetHarvestableStats(HarvestableData.HarvestableType harvestableType)
        {
            var dataKey = $"{HarvestableStatsKey}{harvestableType}";
            var rawData = new HarvestableStatsData.RawData();

            if (SecurePlayerPrefs.HasKey(dataKey))
            {
                var json = SecurePlayerPrefs.GetString(dataKey);
                rawData = JsonUtility.FromJson<HarvestableStatsData.RawData>(json);
            }
            else
            {
                rawData.typeId = (int)harvestableType;
                rawData.spawnedCount = 0;
                rawData.scoreEarned = "0";
            }

            return new HarvestableStatsData(rawData);
        }

        public static void SaveRemoteDataCache(ConfigRemoteService.RemoteData remoteData)
        {
            var json = JsonUtility.ToJson(remoteData);
            Debug.Log(json);
            SecurePlayerPrefs.SetString(RemoteDataKey, json);
        }

        public static ConfigRemoteService.RemoteData GetRemoteDataCache()
        {
            string json;
            if (SecurePlayerPrefs.HasKey(RemoteDataKey))
            {
                json = SecurePlayerPrefs.GetString(RemoteDataKey);
            }
            else
            {
                var textAsset = Resources.Load("defaultHarvestableData") as TextAsset;
                json = textAsset ? textAsset.text : "";
            }

            var remoteData = JsonUtility.FromJson<ConfigRemoteService.RemoteData>(json);
            return remoteData;
        }

        public static void SaveQuality(int id)
        {
            SecurePlayerPrefs.SetInt(QualityKey, id);
        }

        public static int GetQuality()
        {
            return SecurePlayerPrefs.GetInt(QualityKey, 1);
        }

        public static void SaveMusicVolume(float volume)
        {
            SecurePlayerPrefs.SetFloat(MusicKey, volume);
        }

        public static void SaveSfxVolume(float volume)
        {
            SecurePlayerPrefs.SetFloat(SfxKey, volume);
        }

        public static float GetMusicVolume()
        {
            return SecurePlayerPrefs.GetFloat(MusicKey, 1f);
        }
        
        public static float GetSfxVolume()
        {
            return SecurePlayerPrefs.GetFloat(SfxKey, 1f);
        }
    }
}