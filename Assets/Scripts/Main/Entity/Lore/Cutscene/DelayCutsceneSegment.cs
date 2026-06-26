using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Main.Entity.Lore.Cutscene
{
    [Serializable]
    public class DelayCutsceneSegment : CutsceneSegment
    {
        [field: SerializeField] public float DelayTimeInSeconds { get; private set; }

        public override void Execute()
        {
            Wait();
        }

        private async void Wait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DelayTimeInSeconds));
            Proceed();
        }
    }
}