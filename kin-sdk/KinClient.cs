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
        /// <param name="appId">A 3/4 charcter string which represent the application id to add to each transaction</param>
        public KinClient(Environment environment, IKeyStoreProvider keyStore, string appId=DefualtAppId)
        {
            this.environment = environment ?? throw new ArgumentException("Environemnt can't be null");
            ValidateAppId(appId);
            this.appId = appId;
            this.server = InitServer();
            this.keyStore = keyStore;
            this.accountInfoRetriver = new AccountInfoRetriver(this.server);
            this.generalBlockchainInfoRetriever = new GeneralBlockchainInfoRetriever(this.server);
            this.transactionSender = new TransactionSender(this.server, this.appId);
        }

        /// <summary>
        /// Generate a new Keypair and send it the keystore provider for storage
        /// </summary>
        /// <param name="extras">A dictonary of extra attributes to pass to the keystore</param>
        /// <returns></returns>
        public async Task<KeyPair> AddAccount(Dictionary<string, object> extras = null)
        {
            KeyPair keyPair = KeyPair.Random();
            await this.keyStore.AddAccount(keyPair, extras);
            return keyPair;
        }

        public KinAccount GetAccount(KeyPair keyPair)
        {
            return new KinAccount(keyPair, this);
        }

        private Server InitServer()
        {
            Network.Use(this.environment.GetNetwork());
            HttpClient httpClient = Server.CreateHttpClient();
            httpClient.Timeout = TransactionTimeout;
            return new Server(this.environment.networkUrl);
        }

        private void ValidateAppId(string appId)
        {
            if (!String.IsNullOrEmpty(appId))
            {
                if (!Regex.IsMatch(appId, "[a-zA-Z0-9]{3,4}"))
                {
                    throw new ArgumentException($"appId {appId} is invalid, an appId can only contain letters and digits, and be 3 or 4 characters long");
                }
            }
        }
    }
}
