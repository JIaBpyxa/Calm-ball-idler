using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Game;

namespace Vorval.CalmBall.Service
{
    public class SaveService
    {
        private const string ScoreKey = "Score";
        private const string HarvestableKey = "Harvestable_";

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
            var json = JsonUtility.ToJson(harvestableUpgradeData);
            SecurePlayerPrefs.SetString($"{HarvestableKey}{harvestableUpgradeData.Type}", json);
        }

        public static HarvestableUpgradeData GetHarvestableUpgradeData(HarvestableType harvestableType)
        {
            var dataKey = $"{HarvestableKey}{harvestableType}";
            if (!SecurePlayerPrefs.HasKey(dataKey))
            {
                return new HarvestableUpgradeData(harvestableType, false, 0, 0);
            }

            var json = SecurePlayerPrefs.GetString(dataKey);
            var data = JsonUtility.FromJson<HarvestableUpgradeData>(json);
            return data;
        }
    }
}