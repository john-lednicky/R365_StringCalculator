namespace StringCalculator.WorkerService
{
    using StringCalculator.Common;
    using StringCalculator.Services;

    public class Worker(
        StringCalculatorOptions opts,
        IStringCalculator stringCalculator, 
        ILogger<Worker> logger,
        IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var result = stringCalculator.Add(
                    opts.Input,
                    opts.Delimiter,
                    opts.AllowNegatives,
                    opts.NumberLimit
                    );
                logger.LogInformation(result);
            }
            catch (FormatException ex) {
                logger.LogError(ex.Message);
            }

            hostApplicationLifetime.StopApplication();
        }
    }
}
