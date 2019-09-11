using System;
using System.Threading.Tasks;
using Kin.Sdk;
using Kin.Base;

namespace Kin.Sdk.Sample
{
    class Program
    {
        async static Task Main(string[] args)
        {
            KinClient kinClient = new KinClient(Environment.Test, null);
            KinAccount account = kinClient.GetAccount(KeyPair.FromAccountId("GDFH7AUCZQKKMNWO2GIKBK7BAQBUC6FMEPW7IHSRKCZNHPTP5N2DHCTB"));

            while (true)
            {
                var command = Console.ReadLine();
                if ("add" == command)
                {
                    Console.WriteLine("adding listener");
                    account.BalanceListener.OnBalance += BalanceHandler;
                }
                if ("remove" == command)
                {
                    Console.WriteLine("remove listener");
                    account.BalanceListener.OnBalance -= BalanceHandler;
                }
                if ("stop" == command)
                {
                    Console.WriteLine("stop listener");
                    account.BalanceListener.Remove();
                }
                if ("start" == command)
                {
                    Console.WriteLine("start listener");
                    account.BalanceListener.Start();
                    Console.WriteLine("finish starting");
                }
            }
            
        }

        private static void BalanceHandler(decimal balance)
        {
            Console.WriteLine("Got event");
        }
    }
}
