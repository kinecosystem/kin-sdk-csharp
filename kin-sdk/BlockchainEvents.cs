using System;
using Kin.Base;
using Kin.Base.requests;

namespace Kin.Sdk
{
    public class BlockchainEvents
    {
        Server server;
        KeyPair accountKeypair;

        internal BlockchainEvents(Server server, KeyPair accountKeypair)
        {
            this.server = server;
            this.accountKeypair = accountKeypair;
        }

        internal TransactionsRequestBuilder CreateBalanceListener()
        {
            return server.Transactions.ForAccount(this.accountKeypair.AccountId).Cursor("now");
        }
    }
}
