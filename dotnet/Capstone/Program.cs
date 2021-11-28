using System;
using System.IO;
using System.Collections.Generic;
namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine vm = new VendingMachine();
            vm.StockVendingMachine();
            Console.WriteLine("Vendo-Matic 800");
            vm.Display();
        }
    }
}
