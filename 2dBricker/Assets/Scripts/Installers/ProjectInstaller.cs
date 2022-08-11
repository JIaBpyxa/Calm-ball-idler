using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SecurePlayerPrefs.Init();

            Container.Bind<ScoreService>().FromNew().AsSingle();
            Container.Bind<SaveService>().FromNew().AsSingle();
        }
    }
}