using System;

namespace kin_sdk
{
    public class AccountNotFoundException : Exception
    {
        public readonly string accountId;
        public AccountNotFoundException(string accountId)
         : base($"Account {accountId} was not found")
        {
            this.accountId = accountId;
        }
    }
}
