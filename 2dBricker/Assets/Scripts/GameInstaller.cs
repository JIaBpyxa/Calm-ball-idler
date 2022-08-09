using UnityEngine;
using Zenject;

namespace Bricker.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, IHarvestable, IHarvestable.Factory>()
                .FromFactory<PrefabFactory<IHarvestable>>();
        }
    }
}