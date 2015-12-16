using Accounting;
using Accounting.Extensions;
using Budgeteer.Import.Data;
using Budgeteer.Import.PreProcessors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Budgeteer.Controllers
{
    public class LoadTransactionsController: IController
    {
        enum LoadTransactionsState
        {
            Account, File, Progress, Summary
        }

        List<IPreprocessCsvEntries> _preprocessors = new List<IPreprocessCsvEntries>
        {
            new DebetCardPreprocessor(),
            new CreditCardPreprocessor()
        };

        Mutex _stateMutex = new Mutex();
        LoadTransactionsState _state = LoadTransactionsState.Account;

        Account Account { get; set; }
        string FileName { get; set; }
        int Progress { get; set; }

        Mutex _progressMutex = new Mutex();
        Thread _loader;

        List<Transaction> _transactions = new List<Transaction>();

        public string Name
        {
            get { return "LoadTransactions"; }
        }

        public bool Execute()
        {
            _stateMutex.WaitOne();

            try
            {
                Render();

                switch (_state)
                {
                    case LoadTransactionsState.Account:
                        ExecuteChooseAccount();
                        break;

                    case LoadTransactionsState.File:
                        ExecuteChooseFile();
                        break;

                    case LoadTransactionsState.Summary:
                        Console.WriteLine("\nPress enter to return to menu");
                        Console.ReadLine();
                        Program.ChangeController("Menu");
                        break;
                }
            }
            finally
            {
                _stateMutex.ReleaseMutex();
            }

            return true;
        }

        private void ExecuteChooseAccount()
        {
            if (BudgeteerData.Accounts.Count == 0)
            {
                Console.WriteLine("\nNo accounts (press enter to return to menu)");
                Console.ReadLine();
                Program.ChangeController("Menu", true);
                return;
            }

            var input = Console.ReadLine();

            var accountIndex = 0;
            if (int.TryParse(input, out accountIndex) && (accountIndex < BudgeteerData.Accounts.Count))
            {
                Account = BudgeteerData.Accounts[accountIndex];
                _state = LoadTransactionsState.File;
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid account index (press enter to retry)");
                Console.ReadLine();
            }
        }

        private void ExecuteChooseFile()
        {
            var input = Console.ReadLine();

            if (File.Exists(input))
            {
                FileName = input;
                _state = LoadTransactionsState.Progress;
                _loader = new Thread(LoadFile);
                _loader.Start();
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid filename!");
                Console.ReadLine();
            }
        }

        public void LoadFile()
        {
            var lines = File.ReadAllLines(FileName).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                var csvEntry = CsvEntry.FromCsvLine(lines[i]);

                if (csvEntry == null)
                    continue;

                var preProcessors = _preprocessors.Where(p => p.Match(csvEntry));

                foreach (var preProcessor in preProcessors)
                {
                    csvEntry = preProcessor.Process(csvEntry);
                }

                _transactions.Add(csvEntry.ToTransaction(Account));

                _progressMutex.WaitOne();
                Progress = GetProgress(i, lines.Count -1);
                _progressMutex.ReleaseMutex();

                //Thread.Sleep(10);
            }

            BudgeteerData.Transactions.AddRange(_transactions);

            _state = LoadTransactionsState.Summary;
            _overdraw = false;
        }

        private int GetProgress(float progress, float maxprogress)
        {
            return (int)((100.0f / maxprogress) * progress);
        }

        bool _overdraw = false;
        public void Render()
        {
            if (!_overdraw)
            {
                Console.Clear();
                Console.WriteLine("Load Transaction");
            }

            switch(_state)
            {
                case LoadTransactionsState.Account:
                    RenderAccounts();
                    Console.Write("Choose Account: ");
                    break;

                case LoadTransactionsState.File:
                    Console.WriteLine("\nAccount: " + Account.Name);
                    Console.Write("Enter Filename: ");
                    break;

                case LoadTransactionsState.Progress:
                    if (!_overdraw)
                    {
                        Console.WriteLine("\nAccount: " + Account.Name);
                        Console.WriteLine("Filename: " + FileName);
                    }
                    _overdraw = true;
                    Console.SetCursorPosition(0, 5);
                    _progressMutex.WaitOne();
                    Console.Write(String.Format("Progress: {0}%", Progress));
                    _progressMutex.ReleaseMutex();                    
                    break;

                case LoadTransactionsState.Summary:
                    Console.WriteLine("\nAccount: " + Account.Name);
                    Console.WriteLine("Filename: " + FileName);
                    Console.WriteLine("Progress: " + Progress + "%");

                    RenderSummary();
                    break;
            }
        }

        private void RenderSummary()
        {
            Console.WriteLine("\nSummary");
            Console.WriteLine("Transactions: " + _transactions.Count);
            Console.WriteLine("Total Income: " + _transactions.GetIncome(Account, DateTime.MinValue, DateTime.MaxValue).Select(t => t.Amount).Sum());
            Console.WriteLine("Total Expences: " + _transactions.GetExpence(Account, DateTime.MinValue, DateTime.MaxValue).Select(t => t.Amount).Sum());
        }

        public void RenderAccounts()
        {
            Console.WriteLine("\nAccounts:");
            var index = 0;
            foreach (var account in BudgeteerData.Accounts)
                Console.WriteLine(String.Format("{0}: {1}", index++, account.Name));
            Console.WriteLine();
        }

        public void Reset()
        {
            
        }
    }
}
