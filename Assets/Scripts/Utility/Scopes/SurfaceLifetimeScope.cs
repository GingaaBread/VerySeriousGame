using Main.Service;
using VContainer;
using VContainer.Unity;

namespace Utility.Scopes
{
    public class SurfaceLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<UpgradeService>().AsSelf();
        }
    }
}