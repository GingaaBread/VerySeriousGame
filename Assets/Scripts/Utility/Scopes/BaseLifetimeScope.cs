using Main.Service;
using VContainer;
using VContainer.Unity;

namespace Utility.Scopes
{
    public class BaseLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerStatService>(Lifetime.Singleton);
            builder.Register<PlayerInventoryService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BatteryService>().AsSelf();
            builder.RegisterEntryPoint<DrillService>().AsSelf();
            builder.RegisterEntryPoint<UpgradeService>();

            builder.Register<DrillActivationMediator>(Lifetime.Singleton);

            builder.Register<InteractionService>(Lifetime.Singleton);
        }
    }
}