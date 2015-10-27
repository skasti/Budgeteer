using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer
{
    public class BudgeteerData
    {
        public static IList<Transaction> Transactions = new List<Transaction>();
        public static IList<Account> Accounts = new List<Account>();
    }
}
