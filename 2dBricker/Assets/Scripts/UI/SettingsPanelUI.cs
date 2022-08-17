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

        [Inject]
        private void Construct(PanelController panelController)
        {
            _panelController = panelController;
        }

        private void Awake()
        {
            UpdateQuality(SaveService.GetQuality());
        }

        private void Start()
        {
            _exitButton.onClick.AddListener(_panelController.ForceClosePanel);
            _lowGraphicsButton.onClick.AddListener(() => UpdateQuality(0));
            _highGraphicsButton.onClick.AddListener(() => UpdateQuality(1));
            _applicationExitButton.onClick.AddListener(Application.Quit);
        }

        private void UpdateQuality(int id)
        {
            if (id == 0)
            {
                _lowGraphicsButton.interactable = false;
                _highGraphicsButton.interactable = true;
            }
            else
            {
                _lowGraphicsButton.interactable = true;
                _highGraphicsButton.interactable = false;
            }

            QualitySettings.SetQualityLevel(id);
            SaveService.SaveQuality(id);
        }
    }
}