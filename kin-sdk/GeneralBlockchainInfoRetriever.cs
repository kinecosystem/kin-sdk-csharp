using System;
using System.Threading.Tasks;
using Kin.Base;
using Kin.Base.responses;
using Kin.Base.responses.page;
using Kin.Base.requests;

namespace Kin.Sdk
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
            Kin.Base.requests.LedgersRequestBuilder requestBuilder = server.Ledgers;
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
