using VContainer;
using VContainer.Unity;

namespace Utility
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {/*
            // Games
            builder.Register<IMinigameEvaluator, SimulatedMinigameEvaluator>(Lifetime.Singleton);
            builder.Register<MinigameSelectionService>(Lifetime.Singleton);

            // Lifecycle
            builder.Register<LifecycleService>(Lifetime.Singleton);
            builder.Register<MinigamePhaseService>(Lifetime.Singleton);
            builder.Register<MovementPhaseService>(Lifetime.Singleton);
            builder.Register<RoundService>(Lifetime.Singleton);

            // Player
            builder.Register<PlayerCreationService>(Lifetime.Singleton);

            // Board
            builder.Register<BoardService>(Lifetime.Singleton);
            builder.Register<MovementService>(Lifetime.Singleton);

            // Other
            builder.RegisterEntryPoint<InitialisationService>();*/
        }
    }
}