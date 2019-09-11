using System;
using Kin.Base;
using Kin.Base.responses;
using Kin.Base.requests;

namespace Kin.Sdk
{
    public abstract class Listener<TResponse, TRequest> 
        where TResponse: Response
        where TRequest: RequestBuilderStreamable<TRequest,TResponse>
    {
        protected IEventSource serverSentEvents;
        protected readonly KinAccount kinAccount;
        public event Action<Exception> OnError;

        protected Listener(KinAccount kinAccount, TRequest request)
        {
            this.kinAccount = kinAccount;
            this.serverSentEvents = request.Stream( (s, e) => 
            {
                HandleResponse(e);
            });
            this.serverSentEvents.Error += (s ,e) => {OnError?.Invoke(new Exception("SSE FAILED BLA BLA" + e));};
        }

        /// <summary>
        /// Starts the listener in the background.
        /// </summary>
        public void Start() => this.serverSentEvents.Connect();

        /// <summary>
        /// Stop the active listener
        /// </summary>
        public void Remove() => this.serverSentEvents.Shutdown();

        protected abstract void HandleResponse(TResponse response);
    }
}
