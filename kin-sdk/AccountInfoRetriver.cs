using System;
using System.Threading.Tasks;
using kin_base;
using kin_base.responses;

namespace kin_sdk
{
    class AccountInfoRetriver
    {
        private static readonly Asset KinAsset = new AssetTypeNative();
        private readonly Server server;
        internal AccountInfoRetriver(Server server)
        {
            this.server = server;
        }
        internal async Task<decimal> GetBalance(string publicAddress)
        {
            try
            {
                AccountResponse accountResponse = await this.server.Accounts.Account(publicAddress);
                foreach (Balance balance in accountResponse.Balances)
                {
                    if (balance.Asset.Equals(KinAsset))
                    {
                        return Decimal.Parse(balance.BalanceString);
                    }
                }
            }
            catch  (kin_base.requests.HttpResponseException e)
            {
                if (e.StatusCode == 404)
                {
                    throw new AccountNotFoundException(publicAddress);
                }    
                else
                {
                    throw new OperationFailedException("Failed to get account balance", e);
                }
            }

            throw new Exception("Never happens"); // The code wont reach here, since we know for a fact that a KinAsset balance exists. However the null-forgiving operator is only supported from c# 8+
        }

        internal async Task<AccountStatus> GetStatus(string publicAddress)
        {
            try
            {
                await GetBalance(publicAddress);
            }
            catch (AccountNotFoundException)
            {
                return AccountStatus.NotCreated;
            }

            return AccountStatus.Created;
        } 
    }
}
