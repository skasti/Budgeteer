using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Extensions;

namespace Budgeteer.Controllers
{
    public class OverviewController: IController
    {

        public string Name
        {
            get { return "Overview"; }
        }

        public bool Execute()
        {
            Console.Clear();
            Console.WriteLine("Accounts:");
            
            foreach (var account in BudgeteerData.Accounts)
            {
                Console.WriteLine(String.Format("{0} ({1}): {2}", account.Name, account.AccountNumber, account.GetBalance(BudgeteerData.Transactions, DateTime.Now)));
            }

            Console.WriteLine("\n(press enter to return to menu)");
            Console.ReadLine();
            Program.ChangeController("Menu", true);
            return true;
        }

        public void Reset()
        {
            
        }
    }
}
