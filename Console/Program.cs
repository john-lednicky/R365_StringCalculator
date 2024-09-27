namespace StringCalculator.Console
{
    using System;
    using System.Diagnostics;
    using StringCalculator.Services;

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string parm = args[0];

                var sc = new StringCalculator();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                var result = sc.Add(parm);
                sw.Stop();
                long elapsedTimeMs = sw.ElapsedMilliseconds;
                Console.WriteLine($"Execution time: {elapsedTimeMs} ms");

                Console.WriteLine(result);
            }
            else
            {
                Console.Error.WriteLine("Please provide exactly one parameter.");
            }
        }
    }
}
