using UnityEngine;
using Vorval.CalmBall.Service;
using Vorval.CalmBall.UI;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingService _loadingService;
        [SerializeField] private ConfigRemoteService configRemoteService;
        [SerializeField] private HarvestableDataService harvestableDataService;
        [SerializeField] private PanelController _panelController;

        public override void InstallBindings()
        {
            SecurePlayerPrefs.Init();

            Container.BindInstance(_loadingService).AsSingle();
            Container.BindInstance(configRemoteService).AsSingle();
            Container.BindInstance(_panelController).AsSingle();
            Container.BindInstance(harvestableDataService).AsSingle();

            Container.Bind<ScoreService>().FromNew().AsSingle();
            Container.Bind<SaveService>().FromNew().AsSingle();
            Container.Bind<StatsService>().FromNew().AsSingle();

            Container.BindFactory<Object, AbstractPanelUI, AbstractPanelUI.Factory>()
                .FromFactory<PrefabFactory<AbstractPanelUI>>();
        }
    }
}