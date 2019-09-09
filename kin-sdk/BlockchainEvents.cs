using System;
using kin_base;
using kin_base.requests;

namespace kin_sdk
{
    public class BlockchainEvents
    {
        Server server;
        KeyPair accountKeypair;

        public delegate void MyEventHandler(decimal balance);
        public event MyEventHandler BalanceHappened;

        internal BlockchainEvents(Server server, KeyPair accountKeypair)
        {
            this.server = server;
            this.accountKeypair = accountKeypair;
        }

        internal TransactionsRequestBuilder CreateBalanceListener()
        {
            return server.Transactions.ForAccount("GABZ47TJU64H5VCVS5K5FFLT32ZX7NOE26J6RKYMNF6WP745H7UMUVQW").Cursor("now");
        }
    }
}
