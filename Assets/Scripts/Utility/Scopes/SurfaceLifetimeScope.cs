using Main.Mono.Interactions;
using Main.Mono.Player;
using Main.Mono.Surface;
using Main.View.Shop;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utility.Scopes
{
    public class SurfaceLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<RechargeStation>();
            builder.RegisterComponentInHierarchy<ShopView>();
            builder.RegisterComponentInHierarchy<PlayerMovement>();
            builder.RegisterBuildCallback(resolver =>
            {
                foreach (var interactionPoint in FindObjectsByType<InteractionPoint>(FindObjectsSortMode.None))
                {
                    resolver.Inject(interactionPoint);
                }
            });
        }
    }
}