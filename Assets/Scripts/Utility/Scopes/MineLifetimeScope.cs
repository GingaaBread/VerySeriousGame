using Audio;
using VContainer;
using VContainer.Unity;

namespace Utility.Scopes
{
    public class MineLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DrillAudioController>();
        }
    }
}