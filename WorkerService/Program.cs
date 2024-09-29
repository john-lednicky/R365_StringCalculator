namespace StringCalculator.WorkerService
{
    using System.CommandLine;
    using Microsoft.Extensions.Hosting;
    using StringCalculator.Common;
    using StringCalculator.Services;
    public class Program
    {
        public static async Task Main(string[] args)
        {

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

                var builder = Host.CreateApplicationBuilder(args);

                // Add services for injection
                builder.Services.AddSingleton<IConfig, Config>();
                builder.Services.AddSingleton<IInputParser, InputParser>();
                builder.Services.AddSingleton<IValidators, Validators>();
                builder.Services.AddSingleton<IStringCalculator, StringCalculator>();

                // Add commandline parameter values for injection
                builder.Services.AddSingleton(new StringCalculatorOptions(
                    inputArgumentValue, 
                    delimiterOptionValue, 
                    allowNegativesOptionValue, 
                    numberLimitOptionValue));

                // Add hosted service
                builder.Services.AddHostedService<Worker>();

                var host = builder.Build();

                await host.RunAsync();

            },
                inputArgument, delimiterOption, allowNegativesOption, numberLimitOption
            );

            await rootCommand.InvokeAsync(args);
        }
    }
}