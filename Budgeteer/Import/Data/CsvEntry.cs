using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Import.Data
{
    public class CsvEntry
    {
        //Dato;Beskrivelse;Rentedato;Ut/Inn;
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public DateTime RateDate { get; private set; }
        public double Amount { get; private set; }

        public CsvEntry(DateTime date, string description, DateTime rateDate, double amount)
        {
            Date = date;
            Description = description;
            RateDate = rateDate;
            Amount = amount;
        }

        public static CsvEntry FromCsvLine(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
                return null;

            var args = line.Split(';');

            if (args.Length < 4)
                return null;

            var date = DateTime.Parse(args[0]);
            var description = args[1];
            var rateDate = String.IsNullOrEmpty(args[2]) ? date : DateTime.Parse(args[2]);
            var amount = double.Parse(args[3]);

            return new CsvEntry(date, description, rateDate, amount);
        }

        public Transaction ToTransaction(Account account)
        {
            return new Transaction(Date, Amount < 0 ? account : null, Amount > 0 ? account : null, Description, Math.Abs(Amount));
        }
    }
}
