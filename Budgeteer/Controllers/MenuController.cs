using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Controllers
{
    public class MenuController: IController
    {
        public string Name
        {
            get { return "Menu"; }
        }

        public void Reset() { }

        public bool Execute()
        {
            Console.Clear();
            ShowMenu();
            var input = Console.ReadLine();

            switch (input)
            {
                case "1": 
                    Program.ChangeController("Overview", true);
                    break;
                case "2":
                    Program.ChangeController("AddAccount", true);
                    break;
                case "3":
                    Program.ChangeController("LoadTransactions", true);
                    break;
                case "4":
                    return false;

                default:
                    Console.WriteLine("Invalid option. Choose 1..4 (press enter to continue)");
                    Console.ReadLine();
                    break;
            }

            return true;
        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Overview");
            Console.WriteLine("2. Add account");
            Console.WriteLine("3. Load transactions");
            Console.WriteLine("4. Exit");
            Console.WriteLine();
        }
    }
}
