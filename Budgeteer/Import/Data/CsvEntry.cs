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
    }
}
