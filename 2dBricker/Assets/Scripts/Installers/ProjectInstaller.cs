using UnityEngine;
using Vorval.CalmBall.Service;
using Vorval.CalmBall.UI;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private HarvestableDataService _harvestableDataService;
        [SerializeField] private PanelController _panelController;

        public override void InstallBindings()
        {
            SecurePlayerPrefs.Init();
            Container.BindInstance(_panelController).AsSingle();
            Container.BindInstance(_harvestableDataService).AsSingle();

            Container.Bind<ScoreService>().FromNew().AsSingle();
            Container.Bind<SaveService>().FromNew().AsSingle();

            Container.BindFactory<Object, AbstractPanelUI, AbstractPanelUI.Factory>()
                .FromFactory<PrefabFactory<AbstractPanelUI>>();
        }
    }
}