namespace StringCalculator.Console
{
    using System;
    using StringCalculator.Services;

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string parm = args[0];
                var sc = new StringCalculator();
                var result = sc.Add(parm);
                Console.WriteLine(result);
            }
            else
            {
                Console.Error.WriteLine("Please provide exactly one parameter.");
            }
        }
    }
}
