namespace kin_sdk
{
    public class WhitelistableTransaction
    {
        public readonly string transactionPayload;
        public readonly string networkPassphrase;

        internal WhitelistableTransaction(string transactionPayload, string networkPassphrase)
        {
            this.transactionPayload = transactionPayload;
            this.networkPassphrase = networkPassphrase;
        }
    }
}
