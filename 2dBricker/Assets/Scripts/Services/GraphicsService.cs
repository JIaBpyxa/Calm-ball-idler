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
            if (quality == Quality.Eco)
            {
                Application.targetFrameRate = Screen.currentResolution.refreshRate / 2;
            }
            else
            {
                Application.targetFrameRate = Screen.currentResolution.refreshRate / 1;
            }
        }

        public enum Quality
        {
            Eco = 0,
            Fancy = 1
        }
    }
}