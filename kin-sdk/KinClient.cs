using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using kin_base;

namespace kin_sdk
{
    public class KinClient
    {
        private const string DefualtAppId = "anon";
        private readonly TimeSpan TransactionTimeout = TimeSpan.FromSeconds(30);
        private Server server;
        internal readonly TransactionSender transactionSender;
        internal readonly AccountInfoRetriver accountInfoRetriver;
        internal readonly GeneralBlockchainInfoRetriever generalBlockchainInfoRetriever;
        public readonly Environment environment;
        public readonly string appId;
        public readonly IKeyStoreProvider keyStore;

        /// <summary>
        /// Build a KinClient object
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        /// <param name="keyStore">An implementation of the IKeyStoreProvider interface</param>
        /// <param name="appId">A 3/4 charcter string (letters/digits) which represent the application id to add to each transaction</param>
        /// <param name="httpClient">Optional httpClient to use to communicate with the blockchain</param>
        public KinClient(Environment environment, IKeyStoreProvider keyStore, string appId = DefualtAppId, HttpClient httpClient=null)
        {
            this.environment = environment ?? throw new ArgumentNullException("Environemnt");
            this.keyStore = keyStore ?? throw new ArgumentNullException("Keystore");
            ValidateAppId(appId);
            this.appId = appId;
            this.server = InitServer(httpClient);
            this.accountInfoRetriver = new AccountInfoRetriver(this.server);
            this.generalBlockchainInfoRetriever = new GeneralBlockchainInfoRetriever(this.server);
            this.transactionSender = new TransactionSender(this.server, this.appId);
        }

        /// <summary>
        /// Generate a new Keypair and send it the keystore provider for storage
        /// </summary>
        /// <param name="extras">A dictonary of extra attributes to pass to the keystore</param>
        /// <returns>The KeyPair that was generated and sent to storage</returns>
        public async Task<KeyPair> AddAccount(Dictionary<string, object> extras = null)
        {
            KeyPair keyPair = KeyPair.Random();
            await this.keyStore.AddAccount(keyPair, extras);
            return keyPair;
        }

        /// <summary>
        /// Get a KinAccount instance using the supplied keypair
        /// </summary>
        /// <param name="keyPair">KeyPair to use for the account</param>
        /// <returns>A KinAccount that can interface with this client's environment</returns>
        public KinAccount GetAccount(KeyPair keyPair)
        {
            return new KinAccount(keyPair ?? throw new ArgumentNullException("keyPair"), this);
        }

        /// <summary>
        /// Get the minimum acceptable fee for a transaction on the blockchain
        /// </summary>
        /// <returns>The minimum fee</returns>
        public async Task<UInt32> GetMinimumFee()
        {
            return await this.generalBlockchainInfoRetriever.GetMinimumFee();
        }

        private Server InitServer(HttpClient httpClient)
        {
            Network.Use(this.environment.GetNetwork());
            if (httpClient == null)
            {
                httpClient = Server.CreateHttpClient();
                httpClient.Timeout = TransactionTimeout;
            }
            return new Server(this.environment.networkUrl, httpClient);
        }

        private void ValidateAppId(string appId)
        {
            if (appId != "")  // App id can also be an empty string.
            {
                if (!Regex.IsMatch(appId, "[a-zA-Z0-9]{3,4}"))
                {
                    throw new ArgumentException($"appId {appId ?? ""} is invalid, an appId can only contain letters and digits, and be 3 or 4 characters long");
                }
            }
        }

        internal BlockchainEvents CreateBlockchainEventsInstance(KeyPair account) 
        {
            return new BlockchainEvents(this.server, account);
        }
    }
}
