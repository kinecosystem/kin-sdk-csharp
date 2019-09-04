/// <summary>
/// Keystoreprovider used for unit tests
/// </summary>

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using kin_base;
using Newtonsoft.Json;

using kin_sdk;

namespace kin_sdk_tests
{
    class UserAccount
    {
        public string PlayerName;
        public string Seed;
        public string PublicAddress;

        public UserAccount()
        { //Used for deseraliztion
        }

        public UserAccount(string name, KeyPair keyPair)
        {
            this.PlayerName = name;
            this.Seed = keyPair.SecretSeed;
            this.PublicAddress = keyPair.Address;
        }
    }
    public class TestingKeystoreProvider : IKeyStoreProvider
    {
        string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "accounts.json");

        private List<UserAccount> GetAllUserAccounts()
        {
            string text = File.ReadAllText(FilePath);
            List<UserAccount> accounts = JsonConvert.DeserializeObject<List<UserAccount>>(text);

            return accounts;
        }

        private void UpdateLocalStorage(List<UserAccount> accounts)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public async Task AddAccount(KeyPair keypair, Dictionary<string, object> extras = null)
        {
            List<UserAccount> accounts = GetAllUserAccounts();
            UserAccount account = new UserAccount(extras?["PlayerName"]?.ToString() ?? "", keypair);

            accounts.Add(account);
            UpdateLocalStorage(accounts);

        }

        public KeyPair GetAccountByPlayerName(string PlayerName)
        {
            // Method not included in the original interface
            List<UserAccount> accounts = GetAllUserAccounts();

            foreach (UserAccount account in accounts)
            {
                if (account.PlayerName.Equals(PlayerName))
                {
                    return KeyPair.FromSecretSeed(account.Seed);
                }
            }
            throw new Exception();

        }

        public async Task<KeyPair> GetAccountAtIndex(int Index)
        {
            return KeyPair.FromSecretSeed(GetAllUserAccounts()[Index].Seed);
        }

        public async Task<KeyPair> GetAccountByAddress(string PublicAddress)
        {
            List<UserAccount> accounts = GetAllUserAccounts();

            foreach (UserAccount account in accounts)
            {
                if (account.PublicAddress.Equals(PublicAddress))
                {
                    return KeyPair.FromSecretSeed(account.Seed);
                }
            }

            throw new Exception();
        }

        public async Task<int> GetAccountCount()
        {
            return GetAllUserAccounts().Count;
        }

        public async Task RemoveAccount(string PublicAddress)
        {
            List<UserAccount> accounts = GetAllUserAccounts();

            foreach (UserAccount account in accounts)
            {
                if (account.PublicAddress.Equals(PublicAddress))
                {
                    accounts.Remove(account);
                    UpdateLocalStorage(accounts);
                    return;
                }
            }

            throw new Exception();
        }

        public async Task<List<KeyPair>> GetAllAccounts()
        {
            List<KeyPair> keyPairs = new List<KeyPair>();
            foreach (UserAccount account in GetAllUserAccounts())
            {
                keyPairs.Add(KeyPair.FromSecretSeed(account.Seed));
            }

            return keyPairs;
        }

        public void CleanStorage()
        {
            // Used at the start and end of each test
            File.Create(FilePath).Close();
            UpdateLocalStorage(new List<UserAccount>());
        }

        public void RemoveStorage()
        {
            // Used at the end of the test case
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
