using System;
using System.Collections.Generic;
using System.IO;
namespace Capstone
{
    public class VendingMachine
    {

        public decimal AmountAvailable { get; private set; } = 0M;

        internal Dictionary<string, Item> Inventory { get; private set; } = new Dictionary<string, Item>();
        internal Dictionary<string, int> SalesReportHoldings { get; private set; } = new Dictionary<string, int>();
        
        //Holds Sales report total sales.
        public decimal totalSales = 0.00M;

        public void SetInventory(string item)
        {
            string[] itemDetails = item.Split("|");
            if (itemDetails[3] == "Chip")
            {
                Chips chipsItem = new Chips(itemDetails[1], decimal.Parse(itemDetails[2]), itemDetails[3]);
                Inventory[itemDetails[0]] = chipsItem;
                SalesReportHoldings[chipsItem.Name] = 0;
            }
            else if (itemDetails[3] == "Candy")
            {
                Candy candyItem = new Candy(itemDetails[1], decimal.Parse(itemDetails[2]), itemDetails[3]);
                Inventory[itemDetails[0]] = candyItem;
                SalesReportHoldings[candyItem.Name] = 0;
            }
            else if (itemDetails[3] == "Drink")
            {
                Drinks drinkItem = new Drinks(itemDetails[1], decimal.Parse(itemDetails[2]), itemDetails[3]);
                Inventory[itemDetails[0]] = drinkItem;
                SalesReportHoldings[drinkItem.Name] = 0;
            }
            else if (itemDetails[3] == "Gum")
            {
                Gum gumItem = new Gum(itemDetails[1], decimal.Parse(itemDetails[2]), itemDetails[3]);
                Inventory[itemDetails[0]] = gumItem;
               SalesReportHoldings[gumItem.Name] = 0;
            }

        }
        public void StockVendingMachine()
        {
            try
            {
                string file = "vendingmachine.csv";
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string fullPath = Path.Combine(projDirectory, file);
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        SetInventory(line);                      
                        
                    }
                    RefreshSalesReportHoldings();
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine("Error reading the file.");
                Console.WriteLine(ex.Message);
            }
        }
        public void Display()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("(1) Display Vending Machine Items");
            Console.WriteLine("(2) Purchase");
            Console.WriteLine("(3) Exit");
            Console.WriteLine("------------------------------------------------------");
            string userInput = Console.ReadLine();
            Console.WriteLine("------------------------------------------------------");

            bool isNumber = int.TryParse(userInput, out int userSelection);
            if (isNumber && (userSelection == 1 || userSelection == 2 || userSelection == 3 || userSelection == 4))
            {
                if (userSelection == 1)
                {
                    VendingMachineItemDisplay();
                    
                }
                else if (userSelection == 2)
                {
                    PurchaseMenu();
                }
                else if (userSelection == 3)
                {
                    Console.WriteLine("Have a great day!\n");
                }
                else if (userSelection == 4)
                {
                    SalesReportMenu();
                }
            }
            else
            {
                Console.WriteLine("Please enter 1, 2 or 3.");
                Display();
            }
        }
        public void PurchaseMenu()
        {
            Console.WriteLine("(1) Feed Money");
            Console.WriteLine("(2) Select Product");
            Console.WriteLine("(3) Finish Transaction");
            Console.WriteLine($"Current Money Provided: ${AmountAvailable}");
            Console.WriteLine("------------------------------------------------------");

            string userInput = Console.ReadLine();
            Console.WriteLine("------------------------------------------------------");
            bool isNumber = int.TryParse(userInput, out int userSelection);
            if (isNumber && (userSelection == 1 || userSelection == 2 || userSelection == 3))
            {
                if (userSelection == 1)
                {
                    FeedMoney();
                }
                else if (userSelection == 2)
                {
                    PurchaseItem();
                }
                else if (userSelection == 3)
                {
                    Console.WriteLine(FinishTransaction());
                    Console.WriteLine("------------------------------------------------------");
                    Display();
                }
            }
            else
            {
                Console.WriteLine("Please enter 1, 2 or 3.");
                PurchaseMenu();
            }
        }
        public void VendingMachineItemDisplay()
        {
            foreach (KeyValuePair<string, Item> item in Inventory)
            {
                if (item.Value.NumberOfItem == 0)
                {
                    SoldOut(item.Value);
                    Console.WriteLine($"{item.Key}: {item.Value.Name}");
                }
                else
                {
                    Console.WriteLine($"{item.Key}: {item.Value.Name} ${item.Value.Price}");
                }
               
            }
            Console.WriteLine("Press any key to return.");
            Console.WriteLine("------------------------------------------------------");
            Console.ReadKey();
            Console.WriteLine();
            Display();
        }
        public void FeedMoney()
        {
            Console.WriteLine("How much would you like to feed in?");
            Console.WriteLine("(1) $1.00");
            Console.WriteLine("(2) $2.00");
            Console.WriteLine("(5) $5.00");
            Console.WriteLine("(0) $10.00");
            Console.WriteLine("(9) Return to previous menu.");
            Console.WriteLine("------------------------------------------------------");

            while (true)
            {
                string userInput = Console.ReadLine();
                bool isNumber = int.TryParse(userInput, out int userSelection);
                if (isNumber && (userSelection == 1 || userSelection == 2 || userSelection == 5 || userSelection == 0 || userSelection == 9))
                {
                    if (userSelection == 1)
                    {
                        AmountAvailable += 1M;
                        FinanceLog("FEED MONEY", 1.00M);
                    }
                    else if (userSelection == 2)
                    {
                        AmountAvailable += 2M;
                        FinanceLog("FEED MONEY", 2.00M);
                    }
                    else if (userSelection == 5)
                    {
                        AmountAvailable += 5M;
                        FinanceLog("FEED MONEY", 5.00M);
                    }
                    else if (userSelection == 0)
                    {
                        AmountAvailable += 10M;
                        FinanceLog("FEED MONEY", 10.00M);
                    }
                    else if (userSelection == 9)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter 1, 2, 5, 0 or 9.");
                       
                }
                Console.WriteLine($"Current amount available ${AmountAvailable}");
                Console.WriteLine("Continue to enter money or press 9 to exit.");
                Console.WriteLine("------------------------------------------------------");
            }
            PurchaseMenu(); 
        }

        public void PurchaseItem()
        {
            foreach (KeyValuePair<string, Item> item in Inventory)
            {
                if (item.Value.NumberOfItem == 0)
                {
                    SoldOut(item.Value);
                    Console.WriteLine($"{item.Key}: {item.Value.Name}");
                }
                else
                {
                    Console.WriteLine($"{item.Key}: {item.Value.Name} ${item.Value.Price}");
                }
                
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("Please input a product code.");
            Console.WriteLine("------------------------------------------------------");
            string userInput = Console.ReadLine();
            char letter = char.Parse(userInput.Substring(0, 1).ToUpper());
            userInput = letter.ToString() + userInput.Substring(1);

            if (Inventory.ContainsKey(userInput) && Inventory[userInput].NumberOfItem > 0)
            {
                if (Inventory[userInput].Price <= AmountAvailable)
                {
                    AmountAvailable -= Inventory[userInput].Price;
                    Inventory[userInput].DispenseItem();
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine($"Purchased {Inventory[userInput].Name} for ${Inventory[userInput].Price}. Remaining money: ${AmountAvailable}");
                    Console.WriteLine(Inventory[userInput].EatingSounds());
                    FinanceLog(userInput);

                    string nameOfItem = Inventory[userInput].Name;
                    SalesReportHoldings[nameOfItem] = SalesReportHoldings[nameOfItem] + 1;
                    //Calls method that overwrites previously occuring sales report with the updated one.
                    RefreshSalesReport(Inventory[userInput]);
                }
                else if (Inventory[userInput].Price > AmountAvailable)
                {
                    Console.WriteLine("Insufficient funds.\n");
                }
                PurchaseMenu();
            }
            else if (Inventory.ContainsKey(userInput) && Inventory[userInput].NumberOfItem == 0)
            {
                Console.WriteLine("Item is sold out.\n");
                PurchaseMenu();
            }
            else
            {
                Console.WriteLine("Invalid product code, please try again.\n");
                PurchaseMenu();
            }
            Console.WriteLine("------------------------------------------------------");
        }
        public string FinishTransaction()
        {
            const decimal quarter = 25;
            const decimal dime = 10;
            const decimal nickel = 5;
            int quarterCount = 0;
            int dimeCount = 0;
            int nickelCount = 0;
            FinanceLog();
            if (AmountAvailable > 0)
            {
                AmountAvailable = AmountAvailable * 100;
                
                while(AmountAvailable >= quarter)
                {
                    AmountAvailable = AmountAvailable - quarter;
                    quarterCount++;
                }
                while (AmountAvailable >= dime)
                {
                    AmountAvailable = AmountAvailable - dime;
                    dimeCount++;
                }
                while (AmountAvailable >= nickel)
                {
                    AmountAvailable = AmountAvailable - nickel;
                    nickelCount++;
                }
                AmountAvailable = 0;  
            }
            return $"Here is your change: Quarters {quarterCount} Dimes: {dimeCount} Nickels: {nickelCount}";
        }
        public void FinanceLog(string lineItem)
        {
            try
            {
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string file = "Log.txt";
                string fullPath = Path.Combine(projDirectory, file);
                decimal amountBeforePurchase = AmountAvailable + Inventory[lineItem].Price;
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine($"{ DateTime.Now } {Inventory[lineItem].Name}: ${amountBeforePurchase} ${AmountAvailable}");
                }     
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error writing the file.");
                Console.WriteLine(ex.Message);
            }
        }
        public void FinanceLog(string lineItem, decimal amountChanged)
        {
            try
            {
                
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string file = "Log.txt";
                string fullPath = Path.Combine(projDirectory, file);
                ;
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                   sw.WriteLine($"{ DateTime.Now } {lineItem}: ${amountChanged} ${AmountAvailable}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error writing the file.");
                Console.WriteLine(ex.Message);
            }
        }
        public void FinanceLog()
        {
            try
            {
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string file = "Log.txt";
                string fullPath = Path.Combine(projDirectory, file);

                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                   sw.WriteLine($"{DateTime.Now} GIVE CHANGE: ${AmountAvailable} $0.00");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error writing the file.");
                Console.WriteLine(ex.Message);
            }
        }
        public void SalesReportMenu()
        {
            Console.WriteLine("Please input PIN");
            string userInput = Console.ReadLine();
            if (userInput == "1234")
            {
                try
                {
                    string directory = Environment.CurrentDirectory;
                    string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                    string outputFile = "SalesReport.txt";
                    string fullOutputPath = Path.Combine(projDirectory, outputFile);

                    using (StreamReader streamReader = new StreamReader(fullOutputPath))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            string line = streamReader.ReadLine();
                            Console.WriteLine(line);
                        }
                    }

                }
                catch(IOException ex)
                {
                    Console.WriteLine("Error writing the file.");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Access denied.");
                Display();
            }
        }
        public static void SoldOut(Item item)
        {
            item.Name = "SOLD OUT";
        }
        public void RefreshSalesReport(Item item)
        {
            try
            {
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string outputFile = "SalesReport.txt";
                string fullOutputPath = Path.Combine(projDirectory, outputFile);
               

                using (StreamWriter streamWriter = new StreamWriter(fullOutputPath, false))
                {
                    foreach (KeyValuePair<string, int> sale in SalesReportHoldings)
                    {
                        streamWriter.WriteLine($"{sale.Key}|{sale.Value}");
                        totalSales += (item.Price * sale.Value);
                    }
                    streamWriter.WriteLine($"TOTAL SALES: ${totalSales}");
                    totalSales = 0;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error writing the file.");
                Console.WriteLine(ex.Message);
            }
        }
        public void RefreshSalesReportHoldings()
        {
            try
            {
                string directory = Environment.CurrentDirectory;
                string projDirectory = Directory.GetParent(directory).Parent.Parent.Parent.FullName;
                string inputFile = "SalesReport.txt";
                string fullOutputPath = Path.Combine(projDirectory, inputFile);
                using (StreamReader streamReader = new StreamReader(fullOutputPath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if(line.Contains("TOTAL SALES:"))
                        {
                            totalSales = decimal.Parse(line.Substring(14));
                            break;
                        }
                        string[] holdingArray = line.Split("|");
                        SalesReportHoldings[holdingArray[0]] = int.Parse(holdingArray[1]);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error writing the file.");
                Console.WriteLine(ex.Message);
            }   
        }
    }
}
