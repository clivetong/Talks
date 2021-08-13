using System;

namespace TheInnerLoop
{
    static class Program
    {
        static decimal _money = 0;

        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine($"Total money so far: {_money}");
                Console.ReadLine();
                AddPurchase();
            }
        }

        private static void AddPurchase()
        {
            _money += 0.1m; // Damn: wrong ticket price!
        }
    }
}
