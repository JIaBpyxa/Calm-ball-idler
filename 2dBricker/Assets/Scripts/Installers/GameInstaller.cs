using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private WindService _windService;
        [SerializeField] private DayTimeService dayTimeService;

        public override void InstallBindings()
        {
            Container.BindInstance(_cameraService).AsSingle();
            Container.BindInstance(_windService).AsSingle();
            Container.BindInstance(dayTimeService).AsSingle();

            Container.BindFactory<Object, AbstractHarvestable, AbstractHarvestable.Factory>()
                .FromFactory<PrefabFactory<AbstractHarvestable>>();
            Container.BindFactory<Object, ObstaclePack, ObstaclePack.Factory>()
                .FromFactory<PrefabFactory<ObstaclePack>>();
        }
    }
}