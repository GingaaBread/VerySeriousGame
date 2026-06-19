using NUnit.Framework;
using UnityEngine;

namespace Main.View
{
    public abstract class QueuedView : MonoBehaviour
    {
        private FrontEndQueue _callback;

        public void Register(FrontEndQueue queue)
        {
            Assert.IsNull(_callback, "Queued View is already registered");
            _callback = queue;
        }

        protected void MarkAsComplete()
        {
            _callback.Advance(this);
        }
    }
}