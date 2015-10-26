using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Extensions
{
    public static class State
    {
        public static IEnumerable<Transaction> GetIncome(this Account account, IEnumerable<Transaction> transactions, DateTime from, DateTime to)
        {
            return transactions.Where(t =>
                t.ToAccount == account
                && t.Time >= from
                && t.Time <= to);
        }

        public static IEnumerable<Transaction> GetExpence(this Account account, IEnumerable<Transaction> transactions, DateTime from, DateTime to)
        {
            return transactions.Where(t =>
                t.FromAccount == account
                && t.Time >= from
                && t.Time <= to);
        }

        public static double GetBalance(this Account account, IEnumerable<Transaction> transactions, DateTime time)
        {
            return 
                account.GetIncome(transactions, DateTime.Min, time).Select(t => t.Amount).Sum() - 
                account.GetExpence(transactions, DateTime.Min, time).Select(t => t.Amount).Sum();
        }

        public static double GetBalance(this IEnumerable<Transaction> transactions, Account account, DateTime time)
        {

        }
    }
}
