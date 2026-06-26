using System;
using Main.Mono.Lore;

namespace Main.Entity.Lore.Cutscene
{
    [Serializable]
    public abstract class CutsceneSegment
    {
        public abstract void Execute();

        protected void Proceed()
        {
            CutsceneManager.Instance.ProceedWithNextSegment();
        }
    }
}