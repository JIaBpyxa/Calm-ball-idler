using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;
using AudioType = Vorval.CalmBall.Service.AudioType;

namespace Vorval.CalmBall.UI
{
    [RequireComponent(typeof(Button))]
    public class AudioForButton : MonoBehaviour
    {
        private Button _button;

        private AudioService _audioService;

        [Inject]
        private void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(() => _audioService.PlayEffectUI(AudioType.Button));
        }
    }
}