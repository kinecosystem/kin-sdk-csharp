using System;
using kin_base;

namespace kin_sdk
{
    public class ListenerRegistration
    {
        private EventSource serverSentEvents;

        internal ListenerRegistration(EventSource serverSentEvents)
        {
            this.serverSentEvents = serverSentEvents;
        }

        public void Remove() => this.serverSentEvents.Shutdown();
    }
}
