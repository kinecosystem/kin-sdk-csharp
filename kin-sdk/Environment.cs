using System;
using kin_base;

namespace kin_sdk
{
    public class Environment
    {

        public string NetworkUrl { get; }
        public string NetworkPassphrase { get; }

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
                throw new ArgumentException($"networkUrl {networkUrl ?? ""} is not a valid URL");
            }

            if (String.IsNullOrEmpty(networkPassphrase))
            {
                throw new ArgumentException($"networkPassphrase cannot be null or empty");
            }

            this.NetworkUrl = networkUrl;
            this.NetworkPassphrase = networkPassphrase;
        }

        public static Environment Production
        {
            get
            {
                return new Environment("https://horizon.kinfederation.com", "Kin Mainnet ; December 2018");
            }
        }

        public static Environment Test
        {
            get
            {
                return new Environment("https://horizon-testnet.kininfrastructure.com", "Kin Testnet ; December 2018");
            }
        }

        public bool IsMainNet()
        {
            return Production.NetworkUrl == this.NetworkUrl.Replace("http://", "https://") && Production.NetworkPassphrase == this.NetworkPassphrase;
        }

        public Network GetNetwork()
        {
            return new Network(this.NetworkPassphrase);
        }
    }
}
