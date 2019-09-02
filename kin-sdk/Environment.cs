using System;
using kin_base;

namespace kin_sdk
{
    public class Environment
    {

        public readonly string networkUrl;
        public readonly string networkPassphrase;
        
        /// <summary>
        /// Build an Environment object.
        /// </summary>
        /// <param name="networkUrl"> The URL of the blockchain's horizon </param>
        /// <param name="networkPassphrase"> The passphrase/network id to use</param>
        public Environment(string networkUrl, string networkPassphrase)
        {
            Uri uriResult;
            if (!(Uri.TryCreate(networkUrl, UriKind.Absolute, out uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
            {
                throw new ArgumentException($"networkUrl {networkUrl} is not a valid URL");
            }

            if (String.IsNullOrEmpty(networkPassphrase))
            {
                throw new ArgumentException($"networkPassphrase {networkPassphrase} is not a valid passphrase");
            }

            this.networkUrl = networkUrl;
            this.networkPassphrase = networkPassphrase;
        }

        public static readonly Environment Production = new Environment("https://horizon.kinfederation.com","Kin Mainnet ; December 2018");

        public static readonly Environment Test = new Environment("https://horizon-testnet.kininfrastructure.com",
        "Kin Testnet ; December 2018");

        public bool IsMainNet()
        {
            return Production.networkUrl == this.networkUrl && Production.networkPassphrase == this.networkPassphrase;
        }

        public Network GetNetwork()
        {
            return new Network(this.networkPassphrase);
        }
    }
}
