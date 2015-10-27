using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Controllers
{
    public interface IController
    {
        string Name { get; }
        bool Execute();
        void Reset();
    }
}
