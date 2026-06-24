using Main.Mono.Collected_Items;
using Main.Mono.Interactions;
using Main.Service;
using UnityEngine;
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
            builder.RegisterEntryPoint<UpgradeService>().AsSelf();

            builder.Register<DrillActivationMediator>(Lifetime.Singleton);
            builder.Register<InteractionService>(Lifetime.Singleton);

            builder.RegisterBuildCallback(resolver =>
            {
                foreach (var collectable in FindObjectsByType<Collectable>(FindObjectsSortMode.None))
                {
                    resolver.Inject(collectable);
                }

                foreach (var interactionPoint in FindObjectsByType<InteractionPoint>(FindObjectsSortMode.None))
                {
                    resolver.Inject(interactionPoint);
                }
            });
        }
    }
}