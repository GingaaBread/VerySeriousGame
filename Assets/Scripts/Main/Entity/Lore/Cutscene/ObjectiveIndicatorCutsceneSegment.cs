using System;
using Main.Mono.Lore;
using UnityEngine;

namespace Main.Entity.Lore.Cutscene
{
    [Serializable]
    public class ObjectiveIndicatorCutsceneSegment : CutsceneSegment
    {
        [field: SerializeField] public float X { get; set; }
        [field: SerializeField] public float Y { get; set; }

        public override void Execute()
        {
            ObjectiveIndicatorManager.Instance.SpawnAt(X, Y);
            Proceed();
        }
    }
}