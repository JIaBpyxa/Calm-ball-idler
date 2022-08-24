using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public abstract class AudioLibrary : ScriptableObject
    {
        public abstract AudioClip GetAudioClip(AudioType audioType);
    }
}