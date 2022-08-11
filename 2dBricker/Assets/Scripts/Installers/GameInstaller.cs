using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, AbstractHarvestable, AbstractHarvestable.Factory>()
                .FromFactory<PrefabFactory<AbstractHarvestable>>();
            Container.BindFactory<Object, ObstaclePack, ObstaclePack.Factory>()
                .FromFactory<PrefabFactory<ObstaclePack>>();
        }
    }
}