using System;
using System.Threading.Tasks;
using kin_base;

namespace kin_sdk
{
    public class KinAccount
    {
        public readonly KeyPair keyPair; 
        private readonly KinClient client;
        private BlockchainEvents blockchainEvents;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.keyPair = keyPair;
            this.client = client;
            this.blockchainEvents = this.client.CreateBlockchainEventsInstance(keyPair);
        }

        public string GetPublicAddress => this.keyPair.AccountId;

        public Task<AccountStatus> GetStatus()
        {
            return this.client.accountInfoRetriver.GetStatus(this.GetPublicAddress);
        } 

        public Task<decimal> GetBalance() 
        {
            return this.client.accountInfoRetriver.GetBalance(this.GetPublicAddress);
        }

        public async Task<string> SendKin(string destination, decimal amount, UInt32 fee, string memo=null)
        {
            return await this.client.transactionSender.SendKin(this.keyPair, destination, amount, fee, memo);
        }

        public async Task<ListenerRegistration> AddBalanceListener(Action<string> callback)
        {

        } 
    }
}
