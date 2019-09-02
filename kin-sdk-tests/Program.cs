using System;
using kin_base;
using System.Threading.Tasks;
using kin_base.responses;
using kin_sdk;

namespace kin_sdk_tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            KinClient kinClient = new KinClient(kin_sdk.Environment.Test, new DesktopKeyStoreProvider());
            var seed = "SAPFA66BZSEC3DJXYGNWCWCGWMSDGFXRHTEDFDJFZDLJ7ZFHQVEHONXY";
            KinAccount kinAccount = kinClient.GetAccount(KeyPair.FromSecretSeed(seed));
            var balance = await kinAccount.GetBalance();
            Console.WriteLine($"Balance is {balance.ToString()}");
        }
    }
}
