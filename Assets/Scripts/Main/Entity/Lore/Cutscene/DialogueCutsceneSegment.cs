using System;
using Main.View;
using UnityEngine;

namespace Main.Entity.Lore.Cutscene
{
    [Serializable]
    public class DialogueCutsceneSegment : CutsceneSegment
    {
        [SerializeField] private string[] _dialogueLines;

        public override void Execute()
        {
            DialogueCanvas.Instance.StartDialogue(_dialogueLines, true);
        }
    }
}