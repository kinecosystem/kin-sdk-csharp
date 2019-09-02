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

        public string GetPublicAddress() => this.keyPair.AccountId;

        public async Task<AccountStatus> GetStatus()
        {
            return await this.client.accountInfoRetriver.GetStatus(this.GetPublicAddress());
        } 

        public async Task<decimal> GetBalance() 
        {
            return await this.client.accountInfoRetriver.GetBalance(this.GetPublicAddress());
        }
    }
}
