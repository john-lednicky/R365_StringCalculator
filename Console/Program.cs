namespace StringCalculator.Console
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using CommandLine;
    using StringCalculator.Services;
    using StringCalculator.Common;
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton<IConfig, Config>()
                .AddSingleton<IInputParser, InputParser>()
                .AddSingleton<IValidators, Validators>()
                .AddSingleton<StringCalculator>()
                .BuildServiceProvider();

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var sc = services.GetRequiredService<StringCalculator>();
                       var result = sc.Add(o.Input, o.Delimiter, o.AllowNegatives, o.NumberLimit);
                       Console.WriteLine(result);
                   });
        }
    }

    internal class Options
    {
        #pragma warning disable CS8618 // String might be null warning not needed because of attribute default

        [Value(0, Required = true, MetaName = "input", HelpText = "Input string")]
        public string Input { get; set; }

        [Option('d', "delimiter", Default="\n", HelpText = "Alternate delimiter")]
        public string Delimiter { get; set; }

        #pragma warning restore CS8618 // String might be null warning not needed because of attribute default

        [Option('n', "allowNegatives", Default=false, HelpText = "Allow negative numbers")]
        public bool AllowNegatives { get; set; }

        [Option('l', "numberLimit", Default = 1000, HelpText = "Maximum number limit")]
        public int NumberLimit { get; set; }
    }
}
