using System;
using System.Threading.Tasks;
using kin_base;

namespace kin_sdk
{
    public class KinAccount
    {
        public readonly KeyPair keyPair; 
        private readonly KinClient client;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.keyPair = keyPair;
            this.client = client;
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
    }
}
