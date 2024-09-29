namespace StringCalculator.Console
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.CommandLine;
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

            var inputArgument = new Argument<string>(
                name: "input",
                description: "Numbers to process with optional prefix to define delimiters."
                );

            var delimiterOption = new Option<string>(
                name: "--delimiter",
                description: "Alternate delimiter to use instead of newline.",
                getDefaultValue: () => "/n"
                );
            delimiterOption.AddAlias("-d");

            var allowNegativesOption = new Option<bool>(
                name: "--allowNegatives",
                description: "Allow negatives? If not specified, an exception will be thrown when a negative number is encountered in the input.",
                getDefaultValue: () => false
                );
            allowNegativesOption.AddAlias("-n");

            var numberLimitOption = new Option<int>(
                name: "--numberLimit",
                description: "Highest number to process. Numbers in input higher than this are converted to zero.",
                getDefaultValue: () => 1000
                );
            numberLimitOption.AddAlias("-l");

            var rootCommand = new RootCommand();
            rootCommand.Add(inputArgument);
            rootCommand.Add(delimiterOption);
            rootCommand.Add(allowNegativesOption);
            rootCommand.Add(numberLimitOption);

            rootCommand.SetHandler(async (inputArgumentValue, delimiterOptionValue, allowNegativesOptionValue, numberLimitOptionValue) => {

                var sc = services.GetRequiredService<StringCalculator>();
                var result = sc.Add(inputArgumentValue, delimiterOptionValue, allowNegativesOptionValue, numberLimitOptionValue);
                Console.WriteLine(result);
            },
                inputArgument, delimiterOption, allowNegativesOption, numberLimitOption
            );

            rootCommand.InvokeAsync(args);

        }
    }

}
