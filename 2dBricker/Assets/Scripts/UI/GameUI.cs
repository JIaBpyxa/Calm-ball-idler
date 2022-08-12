using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _shopButton;

        private PanelController _panelController;

        [Inject]
        private void Construct(PanelController panelController)
        {
            _panelController = panelController;
        }

        private void Awake()
        {
            if (_settingsButton != null)
            {
                _settingsButton.onClick.AddListener(OpenSettings);
            }

            if (_shopButton != null)
            {
                _shopButton.onClick.AddListener(OpenShop);
            }
        }

        private void OpenSettings()
        {
            _panelController.OpenPanel(PanelController.PanelType.Settings);
        }

        private void OpenShop()
        {
            _panelController.OpenPanel(PanelController.PanelType.Shop);
        }
    }
}