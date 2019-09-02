using System;
using kin_base;

namespace kin_sdk
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
