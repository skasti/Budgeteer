using Budgeteer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer
{
    public class Program
    {
        static IEnumerable<IController> Controllers = new List<IController>
        {
        };

        static IController currentController;
        static IController nextController;
        static bool resetNext = false;

        public static void ChangeController(string controllerName, bool reset)
        {
            var controller = Controllers.FirstOrDefault(c => c.Name == controllerName);

            if (controller == null)
                throw new ArgumentException("Could not find controller '" + controllerName + "'");

            nextController = controller;
            resetNext = reset;
        }

        static void Main(string[] args)
        {

            var running = true;

            while (running)
            {
                try
                {
                    running = currentController.Execute();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }

                SwapControllers();
            }
        }

        private static void SwapControllers()
        {
            if (nextController != null)
            {
                currentController = nextController;
                if (resetNext) currentController.Reset();

                nextController = null;
            }
        }
    }
}
