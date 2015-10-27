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

        public string Name
        {
            get { return "LoadTransactions"; }
        }

        public bool Execute()
        {
            
        }

        public void Reset()
        {
            
        }
    }
}
