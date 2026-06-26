using UnityEngine;

namespace Main.Entity.Lore.Cutscene
{
    [CreateAssetMenu(menuName = "Data/Cutscene/Cutscene", fileName = "New Cutscene")]
    public class CutsceneSO : ScriptableObject
    {
        [SerializeReference]
        [SubclassSelector]
        private CutsceneSegment[] _segments;

        public int AmountOfSegments => _segments.Length;

        public CutsceneSegment SegmentAt(int currentSegmentIndex) => _segments[currentSegmentIndex];
    }
}