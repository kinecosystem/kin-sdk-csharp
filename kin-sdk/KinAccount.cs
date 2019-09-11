using System;
using System.Threading.Tasks;
using Kin.Base;

namespace Kin.Sdk
{
    public class KinAccount
    {
        internal KeyPair keyPair {get;} 
        private readonly KinClient client;
        internal BlockchainEvents blockchainEvents;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.keyPair = keyPair;
            this.client = client;
            this.blockchainEvents = this.client.CreateBlockchainEventsInstance(keyPair);
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

        public async Task<ListenerRegistration> AddBalanceListener(EventHandler<decimal> listener)
        {
            ListenerRegistration response = this.blockchainEvents.CreateBalanceListener(listener);
            await response.Connect();
            return response;
        }
    }
}
