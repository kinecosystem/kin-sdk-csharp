using System;
using Kin.Base.requests;
using Kin.Base.responses;

namespace Kin.Sdk
{
    public class BalanceListener : Listener<TransactionResponse, TransactionsRequestBuilder>
    {

        public event Action<decimal> OnBalance;

        internal BalanceListener(KinAccount kinAccount) 
            :base(kinAccount, kinAccount.blockchainEvents.CreateBalanceListener()) {}

        protected override void HandleResponse(TransactionResponse response)
        {
            OnBalance?.Invoke(50m);
        }
    }
}
