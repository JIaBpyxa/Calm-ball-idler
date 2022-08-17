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

        private PanelController _panelController;
        private GraphicsService _graphicsService;

        [Inject]
        private void Construct(PanelController panelController, GraphicsService graphicsService)
        {
            _panelController = panelController;
            _graphicsService = graphicsService;
        }

        private void Start()
        {
            _exitButton.onClick.AddListener(_panelController.ForceClosePanel);
            _lowGraphicsButton.onClick.AddListener(() => UpdateQuality(GraphicsService.Quality.Eco));
            _highGraphicsButton.onClick.AddListener(() => UpdateQuality(GraphicsService.Quality.Fancy));
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
    }
}