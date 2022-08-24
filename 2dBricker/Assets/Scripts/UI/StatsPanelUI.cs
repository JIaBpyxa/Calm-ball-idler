using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class StatsPanelUI : AbstractPanelUI
    {
        [SerializeField] private Button _exitButton;

        private PanelController _panelController;

        [Inject]
        private void Construct(PanelController panelController)
        {
            _panelController = panelController;
        }

        private void Start()
        {
            _exitButton.onClick.AddListener(_panelController.ForceClosePanel);
        }
    }
}