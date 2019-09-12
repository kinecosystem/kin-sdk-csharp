using System;
using Kin.Base;

namespace Kin.Sdk
{
    class TransactionSender
    {
        private readonly Server server;
        private readonly string appId;
        
        internal TransactionSender(Server server, string appId)
        {
            this.server = server;
            this.appId = appId;
        }
    }
}
