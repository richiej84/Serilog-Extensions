using System;
using System.Threading;
using Serilog.Context;

namespace Serilog.Extensions.Console
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();
             _logger = Log.ForContext<Program>();
            
            //TestOperationContextLogging();
            //System.Console.WriteLine();
            //TestOperationContextLogging_CustomStart();
            //System.Console.WriteLine();
            TestTimedOperation();

            System.Console.WriteLine();
            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        static void TestOperationContextLogging()
        {
            try
            {
                using (_logger.BeginOperation("TestOperationContextLogging"))
                {
                    _logger.Information("Inside log context");
                    throw new Exception();
                }
            }
            catch (Exception)
            {
            }
        }

        static void TestOperationContextLogging_CustomStart()
        {
            using (_logger.BeginOperation("TestOperationContextLogging", "Testing custom start message", options: new OperationContextOptions
            {
                OperationStartedLogWriter = (l, d) => l.Warning("{OperationDescription} (starting)", d.Description)
            }))
            {
                _logger.Information("Inside log context");
            }
        }

        static void TestTimedOperation()
        {
            using (_logger.BeginTimedOperation("TestTimedOperation", new TimedOperationContextOptions { LogMode = OperationContextLogMode.StartAndEnd}))
            {
                Thread.Sleep(1000);
            }
        }

        static void TestTimedOperationExceeding()
        {
            using (_logger.BeginTimedOperation("TestTimedOperationExceeding", TimeSpan.FromSeconds(1)))
            {
                Thread.Sleep(1000);
            }
        }

        static void TestTimedOperationThrowingException()
        {
            try
            {
                using (_logger.BeginOperation("TestTimedOperationThrowingException"))
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
