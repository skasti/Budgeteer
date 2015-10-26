using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    public class Transaction
    {
        public DateTime Time { get; private set; }
        public Account FromAccount { get; private set; }
        public Account ToAccount { get; private set; }
        public String Text { get; private set; }
        public double Amount { get; private set; }

        public Transaction(DateTime time, Account fromAccount, Account toAccount, string text, double amount)
        {
            Time = time;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Text = text;
            Amount = amount;
        }
    }
}
