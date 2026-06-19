using Main.Mono.Collected_Items;
using Main.Service;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerStatService>(Lifetime.Singleton);

            builder.RegisterBuildCallback(resolver =>
            {
                foreach (var collectable in FindObjectsByType<Collectable>(FindObjectsSortMode.None))
                {
                    resolver.Inject(collectable);
                }
            });
        }
    }
}