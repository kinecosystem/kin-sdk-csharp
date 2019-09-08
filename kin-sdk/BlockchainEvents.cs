using System;
using kin_base;

namespace kin_sdk
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
    }
}
