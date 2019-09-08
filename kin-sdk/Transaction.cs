using System;
using kin_base;

namespace kin_sdk
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
        internal readonly kin_base.Transaction baseTransaction;

        internal Transaction(KeyPair source, KeyPair destination, decimal amount, UInt32 fee, string memo, string id, kin_base.Transaction baseTransaction, WhitelistableTransaction whitelistableTransaction)
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
