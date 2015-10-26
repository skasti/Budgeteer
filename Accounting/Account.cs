using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    public class Account
    {
        public string Name { get; private set; }
        public string AccountNumber { get; private set; }

        public Account(string name, string accountNumber)
        {
            Name = name;
            AccountNumber = accountNumber;
        }
    }
}
