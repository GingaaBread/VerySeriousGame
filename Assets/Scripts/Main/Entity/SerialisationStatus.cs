using System.Collections.Generic;

namespace Main.Entity
{
    public class SerialisationStatus
    {
        public List<string> DestroyedObjects { get; set; } = new();
        public int TotalDestroyedObjectCount { get; set; }
    }
}