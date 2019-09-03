using System;
using System.Threading.Tasks;
using kin_base;
using kin_base.responses;
using kin_base.responses.page;
using kin_base.requests;

namespace kin_sdk
{
    class GeneralBlockchainInfoRetriever
    {
        private readonly Server server;
        internal GeneralBlockchainInfoRetriever(Server server)
        {
            this.server = server;
        }

        internal async Task<UInt32> GetMinimumFee()
        {
            kin_base.requests.LedgersRequestBuilder requestBuilder = server.Ledgers;
            requestBuilder.Order(OrderDirection.DESC).Limit(1);
            try
            {
                Page<LedgerResponse> responses = await requestBuilder.Execute();
                return UInt32.Parse(responses.Records[0].BaseFeeInStroops);
            }
            catch (Exception e)
            {
                throw new OperationFailedException("Couldn't retrive minimum fee", e);
            }

        }
    }
}
