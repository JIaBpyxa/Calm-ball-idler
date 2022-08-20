using UnityEngine;

namespace Vorval.CalmBall.Service
{
    [CreateAssetMenu(menuName = "Create GameAudioLibrary", fileName = "GameAudioLibrary", order = 0)]
    public class GameAudioLibrary : AudioLibrary
    {
        public AudioClip simpleSpawned;
        public AudioClip littleSpawned;
        public AudioClip blowSpawned;
        public AudioClip slowSpawned;
        public AudioClip defaultHarvested;
        public AudioClip playerHarvested;
        public AudioClip badHarvested;
        public AudioClip blowHarvested;

        public override AudioClip GetAudioClip(AudioType audioType)
        {
            return audioType switch
            {
                AudioType.SimpleSpawned => simpleSpawned,
                AudioType.LittleSpawned => littleSpawned,
                AudioType.BlowSpawned => blowSpawned,
                AudioType.SlowSpawned => slowSpawned,
                AudioType.DefaultHarvested => defaultHarvested,
                AudioType.PlayerHarvested => playerHarvested,
                AudioType.BadHarvested => badHarvested,
                AudioType.BlowHarvested => blowHarvested,
            };
        }
    }
}