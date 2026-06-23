using Main.Mono.Collected_Items;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility.Scopes
{
    public class MineLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
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