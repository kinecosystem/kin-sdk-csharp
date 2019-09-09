using System;
using kin_base;
using kin_base.requests;
using kin_base.responses;

namespace kin_sdk
{
    public class BalanceListener : Listener<TransactionResponse>
    {

        public event Action<decimal> OnBalance;

        internal BalanceListener(KinAccount kinAccount) :base(kinAccount)
        {
            TransactionsRequestBuilder req = this.kinAccount.blockchainEvents.CreateBalanceListener();
            this.serverSentEvents = req.Stream( (s, e) => 
            {
                //Parse response
                OnBalance(50m);
            });
        }

        protected override void ParseResponse(TransactionResponse response)
        {
            TransactionResponse tx = (TransactionResponse) response;
        }
    }
}
