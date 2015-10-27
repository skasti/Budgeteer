using Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer.Controllers
{
    public class AddAccountController: IController
    {
        enum AddAccountState
        {
            Name, AccountNumber, Summary
        }

        AddAccountState _state = AddAccountState.Name;

        string _accountName = "";
        string _accountNumber = "";

        public string Name
        {
            get { return "AddAccount"; }
        }

        public bool Execute()
        {
            Console.Clear();
            Render();

            switch(_state)
            {
                case AddAccountState.Name:
                    _accountName = Console.ReadLine();
                    
                    if (ValidateInput())
                        _state = AddAccountState.AccountNumber;

                    break;

                case AddAccountState.AccountNumber:
                    _accountNumber = Console.ReadLine();

                    if (ValidateInput())
                        _state = AddAccountState.Summary;

                    break;

                case AddAccountState.Summary:
                    BudgeteerData.Accounts.Add(new Account(_accountName, _accountNumber));
                    Console.ReadLine();
                    Program.ChangeController("Menu", true);
                    break;
            }

            return true;
        }

        public bool ValidateInput()
        {
            switch (_state)
            {
                case AddAccountState.Name:
                    if (String.IsNullOrWhiteSpace(_accountName))
                    {
                        Console.WriteLine("Name must be filled in");
                        return false;
                    }

                    if (BudgeteerData.Accounts.Any(a => a.Name == _accountName))
                    {
                        Console.WriteLine("Name already in use");
                        return false;
                    }
                    
                    break;

                case AddAccountState.AccountNumber:
                    if (String.IsNullOrWhiteSpace(_accountNumber))
                    {
                        Console.WriteLine("Account number must be filled in");
                        return false;
                    }

                    if (BudgeteerData.Accounts.Any(a => a.AccountNumber == _accountNumber))
                    {
                        Console.WriteLine("Account number already in use");
                        return false;
                    }
                    
                    break;
            }

            return true;
        }

        public void Reset()
        {
            _state = AddAccountState.Name;
            _accountName = "";
            _accountNumber = "";
        }

        void Render()
        {
            Console.WriteLine("Add account");
            
            switch(_state)
            {
                case AddAccountState.Name:
                    Console.Write("Name: ");
                    break;

                case AddAccountState.AccountNumber:
                    Console.WriteLine("Name: " + _accountName);
                    Console.Write("Account number: ");
                    break;

                case AddAccountState.Summary:
                    Console.WriteLine("Name: " + _accountName);
                    Console.WriteLine("Account number: " + _accountNumber);
                    Console.WriteLine("\nAccount Created!");
                    Console.WriteLine("\n(press enter to return to menu)");
                    break;
            }
        }
    }
}
