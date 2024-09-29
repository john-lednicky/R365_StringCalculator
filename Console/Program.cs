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
            rootCommand.Add(delimiterOption);
            rootCommand.Add(allowNegativesOption);
            rootCommand.Add(numberLimitOption);

            rootCommand.SetHandler((delimiterOptionValue, allowNegativesOptionValue, numberLimitOptionValue) => {

                var sc = services.GetRequiredService<StringCalculator>();

                Console.WriteLine("Calculator ready!  (ctrl-c or enter to exit)");
                Console.WriteLine($"   delimiter: {delimiterOptionValue}");
                Console.WriteLine($"   negative numbers {(allowNegativesOptionValue ? string.Empty : "not ")}allowed");
                Console.WriteLine($"   numbers over {numberLimitOptionValue} will be converted to zero");
                
                while (true)
                {
                    Console.Write("Calculate this: ");
                    string inputValue = Console.ReadLine() ?? string.Empty;

                    // ctrl-c and OnCancelKeyPress allowed rest of loop to execute before terminating, so we break here if input is empty.
                    if (string.IsNullOrEmpty(inputValue)) break;

                    try
                    {
                        var result = sc.Add(inputValue, delimiterOptionValue, allowNegativesOptionValue, numberLimitOptionValue);
                        Console.WriteLine(result);
                    }
                    catch (FormatException ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            },
                delimiterOption, allowNegativesOption, numberLimitOption
            );

            rootCommand.InvokeAsync(args);

        }
    }
}
