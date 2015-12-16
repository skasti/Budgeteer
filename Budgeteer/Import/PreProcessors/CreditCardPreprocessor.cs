using Budgeteer.Import.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Import.PreProcessors
{
    public class CreditCardPreprocessor: IPreprocessCsvEntries
    {
        public bool Match(Data.CsvEntry csvEntry)
        {
            if (csvEntry.Description.Length < 12)
                return false;

            if (!csvEntry.Description.StartsWith("*"))
                return false;

            var date = csvEntry.Date;
            var dateString = csvEntry.Description.Substring(6, 5) + "." + date.Year.ToString();

            if (!DateTime.TryParse(dateString, out date))
                return false;

            return true;
        }

        public Data.CsvEntry Process(Data.CsvEntry csvEntry)
        {
            var date = csvEntry.Date;
            var dateString = csvEntry.Description.Substring(6, 5) + "." + date.Year.ToString();

            if (!DateTime.TryParse(dateString, out date))
                throw new ArgumentException("DateString '" + dateString + "' does not parse as DateTime");

            return new CsvEntry(date, csvEntry.Description, csvEntry.RateDate, csvEntry.Amount);
        }
    }
}
