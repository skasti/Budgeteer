using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Controllers
{
    public class LoadTransactionsController: IController
    {
        enum LoadTransactionsState
        {
            Account, File, Progress, Summary
        }

        LoadTransactionsState _state = LoadTransactionsState.Account;

        Account Account { get; set; }
        string FileName { get; set; }
        int Progress { get; set; }

        public string Name
        {
            get { return "LoadTransactions"; }
        }

        public bool Execute()
        {
            if (_state != LoadTransactionsState.Progress)
                Console.Clear();

            Render();

            switch (_state)
            {
                case LoadTransactionsState.Account:

                    if (BudgeteerData.Accounts.Count == 0)
                    {
                        Console.WriteLine("\nNo accounts (press enter to return to menu)");
                        Console.ReadLine();
                        Program.ChangeController("Menu", true);
                        break;
                    }

                    var input = Console.ReadLine();

                    var accountIndex = 0;
                    if (int.TryParse(input, out accountIndex) && (accountIndex < BudgeteerData.Accounts.Count))
                    {
                        Account = BudgeteerData.Accounts[accountIndex];
                        _state = LoadTransactionsState.File;
                    }
                    else
                    {
                        Console.WriteLine("\nPlease enter a valid account index (press enter to retry)");
                        Console.ReadLine();
                    }
                    break;
            }

            return true;
        }

        public void Render()
        {
            Console.WriteLine("Load Transaction");

            switch(_state)
            {
                case LoadTransactionsState.Account:
                    RenderAccounts();
                    Console.Write("Choose Account: ");
                    break;

                case LoadTransactionsState.File:
                    Console.WriteLine("\nAccount: " + Account.Name);
                    Console.Write("Enter Filename: ");
                    break;

                case LoadTransactionsState.Progress:
                    Console.SetCursorPosition(0, 5);
                    Console.Write(String.Format("Progress: {0}%", Progress));
                    break;

            }
        }

        public void RenderAccounts()
        {
            Console.WriteLine("\nAccounts:");
            var index = 0;
            foreach (var account in BudgeteerData.Accounts)
                Console.WriteLine(String.Format("{0}: {1}", index++, account.Name));
            Console.WriteLine();
        }

        public void Reset()
        {
            
        }
    }
}
