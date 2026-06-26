using System;
using Main.View;
using UnityEngine;

namespace Main.Entity.Lore.Cutscene

{
    [Serializable]
    public class LetterboxCutsceneSegment : CutsceneSegment
    {
        [SerializeField] private bool _show;

        public override void Execute()
        {
            CutsceneLetterboxCanvas.Instance.Toggle(_show);
            Proceed();
        }
    }
}