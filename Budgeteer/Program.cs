using Budgeteer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Budgeteer
{
    public class Program
    {
        static List<IController> Controllers = new List<IController>();

        static IController currentController;
        static IController nextController;
        static bool resetNext = false;

        public static void ChangeController(string controllerName, bool reset = true)
        {
            var controller = Controllers.FirstOrDefault(c => c.Name == controllerName);

            if (controller == null)
            {
                controller = FindAndInstanciateController(controllerName);
            }

            if (controller == null)
                throw new ArgumentException("Could not find controller '" + controllerName + "'");
            
            nextController = controller;
            resetNext = reset;
        }

        static IController FindAndInstanciateController(string controllerName)
        {
            var typeName = String.Format("Budgeteer.Controllers.{0}Controller", controllerName);
            var type = Assembly.GetExecutingAssembly().GetType(typeName);

            if (type == null) return null;

            var constructor = type.GetConstructor(new Type[0]);

            var controller = constructor.Invoke(new object[0]) as IController;

            Controllers.Add(controller);

            return controller;
        }

        static void Main(string[] args)
        {
            Program.ChangeController("Menu", true);
            SwapControllers();

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
                Thread.Sleep(20);
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
