using System.Resources;

namespace StringCalculator.Services
{
    public static class Config
    {
        public static string? GetErrorMessage(string messageKey) {
            ResourceManager rm = new ResourceManager("Services.ErrorMessages",typeof(Config).Assembly);
            string? returnValue = rm.GetString(messageKey);
            return returnValue;
        }
    }
}
