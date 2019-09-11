using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kin.Base;
using Kin.Base.responses;

namespace Kin.Sdk
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
                if (accountResponse == null)
                {
                    throw new OperationFailedException("Failed to get account balance");
                }

                foreach (Balance balance in accountResponse.Balances)
                {
                    if (balance.Asset.Equals(KinAsset))
                    {
                        return Decimal.Parse(balance.BalanceString);
                    }
                }
                throw new OperationFailedException("Failed to get account balance"); // Not supposed to ever get here.
            }
            catch  (Kin.Base.requests.HttpResponseException e)
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
            catch (HttpRequestException e)
            {
                throw new OperationFailedException("Failed to get account balance", e);
            }
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
