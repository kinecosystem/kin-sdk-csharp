using System;
using System.Threading.Tasks;
using kin_base;

namespace kin_sdk
{
    public class KinAccount
    {
        public KeyPair KeyPair { get; }
        private readonly KinClient client;
        internal BlockchainEvents blockchainEvents;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.KeyPair = keyPair;
            this.client = client;
            this.blockchainEvents = this.client.CreateBlockchainEventsInstance(keyPair);
        }

        public string GetPublicAddress => this.KeyPair.AccountId;

        public Task<AccountStatus> GetStatus()
        {
            return this.client.accountInfoRetriver.GetStatus(this.GetPublicAddress);
        }

        public Task<decimal> GetBalance()
        {
            return this.client.accountInfoRetriver.GetBalance(this.GetPublicAddress);
        }

        public Task<string> SendKin(string destination, decimal amount, UInt32 fee, string memo = null)
        {
            return this.client.transactionSender.SendKin(this.KeyPair, destination, amount, fee, memo);
        }

        public async Task<ListenerRegistration> AddBalanceListener(EventHandler<decimal> listener)
        {
            ListenerRegistration response = this.blockchainEvents.CreateBalanceListener(listener);
            await response.Connect();
            return response;
        }
    }
}
