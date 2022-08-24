using UnityEngine;

namespace Vorval.CalmBall.Service
{
    [CreateAssetMenu(menuName = "Create UIAudioLibrary", fileName = "UI AudioLibrary", order = 0)]
    public class UIAudioLibrary : AudioLibrary
    {
        public AudioClip buttonAudio;
        public AudioClip panelOpen;
        public AudioClip panelClose;
        public AudioClip powerUpgrade;
        public AudioClip respawnUpgrade;
        public AudioClip harvestableBought;

        public override AudioClip GetAudioClip(AudioType audioType)
        {
            return audioType switch
            {
                AudioType.Button => buttonAudio,
                AudioType.PanelOpen => panelOpen,
                AudioType.PanelClose => panelClose,
                AudioType.PowerUpgrade => powerUpgrade,
                AudioType.RespawnUpgrade => respawnUpgrade,
                AudioType.HarvestableBought => harvestableBought
            };
        }
    }
}