using System;
using System.Threading.Tasks;
using Kin.Base;

namespace Kin.Sdk
{
    public class KinAccount
    {
        public BalanceListener BalanceListener { get; }
        internal KeyPair keyPair { get; }
        private readonly KinClient client;
        internal BlockchainEvents blockchainEvents;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.keyPair = keyPair;
            this.client = client;
            this.blockchainEvents = this.client.CreateBlockchainEventsInstance(keyPair);
            this.BalanceListener = new BalanceListener(this);
        }

        public string PublicAddress => this.keyPair.AccountId;

        public Task<AccountStatus> GetStatus()
        {
            return this.client.accountInfoRetriver.GetStatus(this.PublicAddress);
        }

        public Task<decimal> GetBalance()
        {
            return this.client.accountInfoRetriver.GetBalance(this.PublicAddress);
        }

        public Task<string> SendKin(string destination, decimal amount, UInt32 fee, string memo = null)
        {
            return this.client.transactionSender.SendKin(this.keyPair, destination, amount, fee, memo);
        }
    }
}
