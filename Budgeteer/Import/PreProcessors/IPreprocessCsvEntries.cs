using Accounting;
using Budgeteer.Import.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Import.PreProcessors
{
    public interface IPreprocessCsvEntries
    {
        bool Match(CsvEntry csvEntry);
        CsvEntry Process(CsvEntry csvEntry);
    }
}
