using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraService _cameraService;

        public override void InstallBindings()
        {
            Container.BindInstance(_cameraService).AsSingle();

            Container.BindFactory<Object, AbstractHarvestable, AbstractHarvestable.Factory>()
                .FromFactory<PrefabFactory<AbstractHarvestable>>();
            Container.BindFactory<Object, ObstaclePack, ObstaclePack.Factory>()
                .FromFactory<PrefabFactory<ObstaclePack>>();
        }
    }
}