using SettlersOfCatan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SettlersOfCatan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AutoMapperRegister.RegisterMapping();
            var numberOfSimulations = 10;

            Parallel.For(0, numberOfSimulations, 
                new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0)) },
                sim =>
                {
                    Console.WriteLine("{0}, Thread Id={1} START", sim, Thread.CurrentThread.ManagedThreadId);
                    RunBoard(new String[] {"MCTS", "Aggressive"});
                    Console.WriteLine("{0}, Thread Id={1} FINISH", sim, Thread.CurrentThread.ManagedThreadId);
                }
            );
        }

        [STAThread]
        static void RunBoard(object data)
        {
            var board = new Board((string[])data, true);
            Application.Run(board);
        }
    }
}
