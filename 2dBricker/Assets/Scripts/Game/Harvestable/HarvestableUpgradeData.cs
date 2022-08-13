namespace Vorval.CalmBall.Game
{
    public class HarvestableUpgradeData
    {
        public struct RawData
        {
            public int typeId;
            public bool isBought;
            public int powerUpgradeLevel;
            public int respawnIntervalUpgradeLevel;
        }

        public HarvestableType Type { get; }
        public bool IsBought { get; private set; }
        public int PowerUpgradeLevel { get; private set; }
        public int RespawnUpgradeLevel { get; private set; }

        // public HarvestableUpgradeData(HarvestableType type, bool isBought, int powerUpgradeLevel,
        //     int respawnUpgradeLevel)
        // {
        //     Type = type;
        //     IsBought = isBought;
        //     PowerUpgradeLevel = powerUpgradeLevel;
        //     RespawnUpgradeLevel = respawnUpgradeLevel;
        // }

        public HarvestableUpgradeData(RawData rawData)
        {
            Type = (HarvestableType)rawData.typeId;
            IsBought = rawData.isBought;
            PowerUpgradeLevel = rawData.powerUpgradeLevel;
            RespawnUpgradeLevel = rawData.respawnIntervalUpgradeLevel;
        }

        public void UpgradePower()
        {
            PowerUpgradeLevel++;
        }

        public void UpgradeRespawn()
        {
            RespawnUpgradeLevel++;
        }

        public RawData GetRawData()
        {
            return new RawData
            {
                typeId = (int)Type,
                isBought = IsBought,
                powerUpgradeLevel = PowerUpgradeLevel,
                respawnIntervalUpgradeLevel = RespawnUpgradeLevel
            };
        }
    }
}