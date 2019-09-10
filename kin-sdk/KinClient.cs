using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Kin.Base;

namespace Kin.Sdk
{
    public class KinClient
    {
        private const string DefualtAppId = "anon";
        private readonly TimeSpan TransactionTimeout = TimeSpan.FromSeconds(30);
        private readonly Server server;
        internal readonly TransactionSender transactionSender;
        internal readonly AccountInfoRetriver accountInfoRetriver;
        internal readonly GeneralBlockchainInfoRetriever generalBlockchainInfoRetriever;
        public Environment Environment { get; }
        public string AppId { get; }
        public IKeyStoreProvider KeyStore { get; }

        /// <summary>
        /// Build a KinClient object
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        /// <param name="keyStore">An implementation of the IKeyStoreProvider interface</param>
        /// <param name="appId">A 3/4 charcter string (letters/digits) which represent the application id to add to each transaction</param>
        /// <param name="httpClient">Optional httpClient to use to communicate with the blockchain</param>
        public KinClient(Environment environment, IKeyStoreProvider keyStore, string appId = DefualtAppId, HttpClient httpClient = null)
        {
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.KeyStore = keyStore ?? throw new ArgumentNullException(nameof(keyStore));
            ValidateAppId(appId);
            this.AppId = appId;
            this.server = InitServer(httpClient);
            this.accountInfoRetriver = new AccountInfoRetriver(this.server);
            this.generalBlockchainInfoRetriever = new GeneralBlockchainInfoRetriever(this.server);
            this.transactionSender = new TransactionSender(this.server, this.AppId);
        }

        /// <summary>
        /// Generate a new Keypair and send it the keystore provider for storage
        /// </summary>
        /// <param name="extras">A dictonary of extra attributes to pass to the keystore</param>
        /// <returns>The KeyPair that was generated and sent to storage</returns>
        public async Task<KeyPair> AddAccount(Dictionary<string, object> extras = null)
        {
            KeyPair keyPair = KeyPair.Random();
            await this.KeyStore.AddAccount(keyPair, extras);
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
        /// Get the minimum acceptable fee (In Quarks - 1/100,000 Kin) for a transaction on the blockchain
        /// </summary>
        /// <returns>The minimum fee</returns>
        public Task<UInt32> GetMinimumFee()
        {
            return this.generalBlockchainInfoRetriever.GetMinimumFee();
        }

        private Server InitServer(HttpClient httpClient)
        {
            Network.Use(this.Environment.Network);
            if (httpClient == null)
            {
                httpClient = Server.CreateHttpClient();
                httpClient.Timeout = TransactionTimeout;
            }
            return new Server(this.Environment.NetworkUrl, httpClient);
        }

        private void ValidateAppId(string appId)
        {
            if (!String.IsNullOrEmpty(appId))  // App id can also be an empty string or null.
            {
                if (!Regex.IsMatch(appId, "[a-zA-Z0-9]{3,4}"))
                {
                    throw new ArgumentException($"appId {appId} is invalid, an appId can only contain letters and digits, and be 3 or 4 characters long");
                }
            }
        }
    }
}
