using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _panelHolder;
        [Space] [SerializeField] private AbstractPanelUI _settingsPanel;
        [SerializeField] private AbstractPanelUI _shopPanel;
        [SerializeField] private AbstractPanelUI _statsPanel;
        [SerializeField] private AbstractPanelUI _statsDataPanel;
        [SerializeField] private AbstractPanelUI _rewardedAdPanel;

        private AbstractPanelUI _currentPanel;
        private PanelType _previousPanelType = 0;

        private AbstractPanelUI.Factory _panelFactory;

        [Inject]
        private void Construct(AbstractPanelUI.Factory panelFactory)
        {
            _panelFactory = panelFactory;
        }

        private void Start()
        {
            ForceClosePanel();
        }

        public void OpenPanel(PanelType panelType)
        {
            if (panelType == PanelType.None) return;

            var panelPrefab = GetPanelPrefab();
            if (panelPrefab == null) return;

            if (_currentPanel == null)
            {
                _previousPanelType = PanelType.None;
                _canvasGroup.blocksRaycasts = true;
                SpawnPanel(panelPrefab);
            }
            else
            {
                _previousPanelType = _currentPanel.GetPanelType();
                _currentPanel.ClosePanel(() => SpawnPanel(panelPrefab));
            }

            _canvasGroup.DOFade(1f, .5f);


            AbstractPanelUI GetPanelPrefab()
            {
                return panelType switch
                {
                    PanelType.Settings => _settingsPanel,
                    PanelType.Shop => _shopPanel,
                    PanelType.Stats => _statsPanel,
                    PanelType.StatsData => _statsDataPanel,
                    PanelType.RewardedAdPanel => _rewardedAdPanel,
                    _ => null
                };
            }
        }

        public void ForceClosePanel()
        {
            _canvasGroup.DOFade(0f, .3f);
            _canvasGroup.blocksRaycasts = false;
            if (_currentPanel == null) return;
            _currentPanel.ClosePanel();
            _currentPanel = null;
            _previousPanelType = PanelType.None;
        }

        public void GoBackward()
        {
            if (_previousPanelType == PanelType.None)
            {
                ForceClosePanel();
            }
            else
            {
                OpenPanel(_previousPanelType);
            }
        }

        private void SpawnPanel(AbstractPanelUI panelPrefab)
        {
            _currentPanel = _panelFactory.Create(panelPrefab);
            _currentPanel.transform.SetParent(_panelHolder);
            _currentPanel.OpenPanel();
        }

        public enum PanelType
        {
            None = 0,
            Settings = 1,
            Shop = 2,
            Stats = 3,
            StatsData = 4,
            RewardedAdPanel = 5,
        }
    }
}