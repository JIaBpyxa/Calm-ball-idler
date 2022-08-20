using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class GraphicsService : MonoBehaviour
    {
        public Quality CurrentQuality { get; private set; }

        private void Awake()
        {
            CurrentQuality = (Quality)SaveService.GetQuality();
        }

        private void Start()
        {
            UpdateQuality(CurrentQuality);
        }

        public void UpdateQuality(Quality quality)
        {
            var qualityId = (int)quality;
            CurrentQuality = quality;

            QualitySettings.SetQualityLevel(qualityId);
            SaveService.SaveQuality(qualityId);

            UpdateFPS(quality);
        }

        private void UpdateFPS(Quality quality)
        {
            QualitySettings.vSyncCount = 0;
            var refreshRate = Screen.currentResolution.refreshRate;

            if (refreshRate <= 60) refreshRate = 60;

            Application.targetFrameRate = quality == Quality.Fancy ? refreshRate : 60;

            Debug.Log($"Refresh rate {refreshRate} Target FPS {Application.targetFrameRate}");
        }

        public enum Quality
        {
            Eco = 0,
            Fancy = 1
        }
    }
}