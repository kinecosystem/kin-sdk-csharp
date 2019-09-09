using System;
using System.Threading.Tasks;
using kin_sdk;
using kin_base;

namespace kin_sdk_sample
{
    class Program
    {
        private object balanceEventLock = new object();
        private Action<decimal> _balanceEvent;

        private static async Task Foo()
        {
            await Task.Delay(5000);
            Console.WriteLine("Got called");
            //throw new Exception("blbla");
        }

        public event Action<decimal> balanceEvent
        {
            add
            {
                lock (balanceEventLock)
                {
                    Console.WriteLine("added");
                    _balanceEvent += value;
                    Console.WriteLine($"There are now {_balanceEvent.GetInvocationList().Length} subscribers");
                    // Start listener
                }
            }
            remove
            {
                lock (balanceEventLock)
                {
                    Console.WriteLine("removed");
                    balanceEvent -= value;
                }
            }
        }

        async static Task Main(string[] args)
        {
            Program p = new Program();
            p.balanceEvent += (decimal balance) => { Console.WriteLine($"Balance is {balance.ToString()}"); };
            p._balanceEvent?.Invoke(50m);
        }
    }
}
