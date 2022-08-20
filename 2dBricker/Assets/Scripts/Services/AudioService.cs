﻿using System;
using UnityEngine;
using Vorval.CalmBall.Game;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioLibrary uiAudioLibrary;
        [SerializeField] private AudioLibrary gameAudioLibrary;
        [Space] [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsUISource;
        [SerializeField] private AudioSource _effectsGameSource;


        [Inject]
        private void Construct(HarvestableDataService harvestableDataService)
        {
            harvestableDataService.OnPowerUpgrade += _ => PlayEffectUI(AudioType.PowerUpgrade);
            harvestableDataService.OnRespawnUpgrade += _ => PlayEffectUI(AudioType.RespawnUpgrade);
            harvestableDataService.OnHarvestableBought += _ => PlayEffectUI(AudioType.HarvestableBought);
        }

        public void PlayEffectUI(AudioType audioType)
        {
            PlayEffect(_effectsUISource, uiAudioLibrary, audioType);
        }

        public void PlayHarvestedEffect(Harvester.HarvesterType harvestableType)
        {
            var audioType = harvestableType switch
            {
                Harvester.HarvesterType.BottomZone => AudioType.DefaultHarvested,
                Harvester.HarvesterType.Player => AudioType.PlayerHarvested,
                Harvester.HarvesterType.RedZone => AudioType.BadHarvested,
                Harvester.HarvesterType.BlowingHarvestable => AudioType.BlowHarvested,
            };

            PlayEffectGame(audioType);
        }

        public void PlayHarvestableSpawnedEffect(HarvestableData.HarvestableType harvestableType)
        {
            var audioType = harvestableType switch
            {
                HarvestableData.HarvestableType.Simple => AudioType.SimpleSpawned,
                HarvestableData.HarvestableType.Little => AudioType.LittleSpawned,
                HarvestableData.HarvestableType.Blow => AudioType.BlowSpawned,
                HarvestableData.HarvestableType.Slow => AudioType.SlowSpawned,
            };

            PlayEffectGame(audioType, .5f);
        }

        public void PlayEffectGame(AudioType audioType, float volume = 1f)
        {
            PlayEffect(_effectsGameSource, gameAudioLibrary, audioType, volume);
        }

        private void PlayEffect(AudioSource audioSource, AudioLibrary library, AudioType audioType, float volume = 1f)
        {
            var clip = library.GetAudioClip(audioType);
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public enum AudioType
    {
        Button = 1,
        PanelOpen = 2,
        PanelClose = 3,
        PowerUpgrade = 11,
        RespawnUpgrade = 12,
        HarvestableBought = 13,
        SimpleSpawned = 101,
        LittleSpawned = 102,
        BlowSpawned = 103,
        SlowSpawned = 104,
        DefaultHarvested = 111,
        PlayerHarvested = 112,
        BadHarvested = 113,
        BlowHarvested = 114,
    }
}