using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<InputService>().AsSingle();
        Container.Bind<ITimeService>().To<TimeService>().AsSingle();
        Container.Bind<IGameStateService>().To<GameStateService>().AsSingle();
        Container.Bind<IVFXService>().To<VFXService>().AsSingle();
        Container.Bind<IProgressService>().To<ProgressService>().AsSingle();

    }
}
