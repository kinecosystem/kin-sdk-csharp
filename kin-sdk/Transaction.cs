using System;
using Kin.Base;

namespace Kin.Sdk
{
    public class Transaction
    {

        public readonly KeyPair source;
        public readonly KeyPair destination;
        public readonly decimal amount;
        public readonly UInt32 fee;
        public readonly string memo;
        public readonly string id;
        public readonly WhitelistableTransaction whitelistableTransaction;
        internal readonly Kin.Base.Transaction baseTransaction;

        internal Transaction(KeyPair source, KeyPair destination, decimal amount, UInt32 fee, string memo, string id, Kin.Base.Transaction baseTransaction, WhitelistableTransaction whitelistableTransaction)
        {
            this.source = source;
            this.destination = destination;
            this.amount = amount;
            this.fee = fee;
            this.memo = memo;
            this.id = id;
            this.baseTransaction = baseTransaction;
            this.whitelistableTransaction = whitelistableTransaction;
        }
    }
}
