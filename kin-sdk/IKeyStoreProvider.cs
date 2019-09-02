using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kin_base;

namespace kin_sdk
{
    public interface IKeyStoreProvider
    {
        Task AddAccount(KeyPair keypair, Dictionary<string, object> extras = null);
        // Add account to storage

        Task RemoveAccount(string identifier);
        // Remove account from storage

        Task<KeyPair> GetAccountByAddress(string PublicAddress);
        // Get account by public address

        Task<KeyPair> GetAccountAtIndex(int Index);
        // Get account at index

        Task<int> GetAccountCount();
        // Get count of accounts in storage

        Task<List<KeyPair>> GetAllAccounts();
        // Get a list of all the account avilable in storage

    }
}

