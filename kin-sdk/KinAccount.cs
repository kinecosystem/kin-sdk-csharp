using System;
using System.Threading.Tasks;
using Kin.Base;

namespace Kin.Sdk
{
    public class KinAccount
    {
        internal KeyPair keyPair {get;} 
        private readonly KinClient client;

        internal KinAccount(KeyPair keyPair, KinClient client)
        {
            this.keyPair = keyPair;
            this.client = client;
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
    }
}
