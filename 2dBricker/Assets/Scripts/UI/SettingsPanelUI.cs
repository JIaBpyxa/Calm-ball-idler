using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class SettingsPanelUI : AbstractPanelUI
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _lowGraphicsButton;
        [SerializeField] private Button _highGraphicsButton;
        [SerializeField] private Button _applicationExitButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private PanelController _panelController;
        private GraphicsService _graphicsService;
        private AudioService _audioService;

        [Inject]
        private void Construct(PanelController panelController, GraphicsService graphicsService,
            AudioService audioService)
        {
            _panelController = panelController;
            _graphicsService = graphicsService;
            _audioService = audioService;
        }

        private void Start()
        {
            _exitButton.onClick.AddListener(_panelController.ForceClosePanel);
            _lowGraphicsButton.onClick.AddListener(() => UpdateQuality(GraphicsService.Quality.Eco));
            _highGraphicsButton.onClick.AddListener(() => UpdateQuality(GraphicsService.Quality.Fancy));

            _musicSlider.value = _audioService.MusicVolume;
            _sfxSlider.value = _audioService.SfxVolume;

            _musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            _sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);

            _applicationExitButton.onClick.AddListener(Application.Quit);

            UpdateQuality(_graphicsService.CurrentQuality);
        }

        private void UpdateQuality(GraphicsService.Quality quality)
        {
            if (quality == GraphicsService.Quality.Eco)
            {
                _lowGraphicsButton.interactable = false;
                _highGraphicsButton.interactable = true;
            }
            else
            {
                _lowGraphicsButton.interactable = true;
                _highGraphicsButton.interactable = false;
            }

            _graphicsService.UpdateQuality(quality);
        }


        private void UpdateMusicVolume(float volume)
        {
            _audioService.UpdateMusicVolume(volume);
        }

        private void UpdateSfxVolume(float volume)
        {
            _audioService.UpdateSfxVolume(volume);
        }
    }
}