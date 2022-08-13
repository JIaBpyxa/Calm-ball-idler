namespace Vorval.CalmBall.Game
{
    public class HarvestableUpgradeData
    {
        public HarvestableType Type { get; }
        public bool IsBought { get; private set; }
        public int PowerUpgradeLevel { get; private set; }
        public int RespawnUpgradeLevel { get; private set; }

        public HarvestableUpgradeData(HarvestableType type, bool isBought, int powerUpgradeLevel,
            int respawnUpgradeLevel)
        {
            Type = type;
            IsBought = isBought;
            PowerUpgradeLevel = powerUpgradeLevel;
            RespawnUpgradeLevel = respawnUpgradeLevel;
        }

        public void UpgradePower()
        {
            PowerUpgradeLevel++;
        }

        public void UpgradeRespawn()
        {
            RespawnUpgradeLevel++;
        }
    }
}