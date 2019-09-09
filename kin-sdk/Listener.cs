using System;
using System.Threading.Tasks;
using kin_base;
using kin_base.responses;

namespace kin_sdk
{
    public abstract class Listener<T> where T: Response
    {
        protected IEventSource serverSentEvents;
        protected readonly KinAccount kinAccount;

        protected Listener(KinAccount kinAccount)
        {
            this.kinAccount = kinAccount;
        }

        internal Task Connect() => this.serverSentEvents.Connect();

        public void Remove() => this.serverSentEvents.Shutdown();

        protected abstract void ParseResponse(T response);

    }
}
