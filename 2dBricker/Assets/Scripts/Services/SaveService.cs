using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Game;

namespace Vorval.CalmBall.Service
{
    public class SaveService
    {
        private const string ScoreKey = "Score";
        private const string HarvestableKey = "Harvestable_";
        private const string RemoteDataKey = "RemoteData";

        public static void SaveScore(BigInteger score)
        {
            //SecurePlayerPrefs.SetInt(ScoreKey, score);
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
            //return SecurePlayerPrefs.GetInt(ScoreKey, 0);
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

        public static void SaveRemoteDataCache(ConfigRemoteService.RemoteData remoteData)
        {
            var json = JsonUtility.ToJson(remoteData);
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
    }
}