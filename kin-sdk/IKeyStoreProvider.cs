using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kin.Base;

namespace Kin.Sdk
{
    public interface IKeyStoreProvider
    {
        /// <summary>
        /// Add an account to the keystore
        /// </summary>
        /// <param name="keypair">The keypair to save</param>
        /// <param name="extras">Optional dict of extra args to save</param>
        Task AddAccount(KeyPair keypair, Dictionary<string, object> extras = null);

        /// <summary>
        /// Remove an account from the keystore
        /// </summary>
        /// <param name="identifier">The identifer of the account to remove</param>
        Task RemoveAccount(string identifier);

        /// <summary>
        /// Get an account by its public address
        /// </summary>
        /// <param name="PublicAddress">The public address of the account to get</param>
        Task<KeyPair> GetAccountByAddress(string PublicAddress);

        /// <summary>
        /// Get an account by its index in the keystore
        /// </summary>
        /// <param name="Index">Index of the account to get</param>
        Task<KeyPair> GetAccountAtIndex(int Index);

        /// <summary>
        /// Get count of accounts stored in this keystore.
        /// </summary>
        Task<int> GetAccountCount();

        /// <summary>
        /// Get all of the accounts stored in this keystore
        /// </summary>
        Task<List<KeyPair>> GetAllAccounts();

    }
}

