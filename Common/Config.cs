using System.Resources;

namespace StringCalculator.Common
{
    public class Config : IConfig
    {
        public string? GetErrorMessage(string messageKey) {
            ResourceManager rm = new ResourceManager("Common.ErrorMessages",typeof(Config).Assembly);
            string? returnValue = rm.GetString(messageKey);
            return returnValue;
        }
    }
}
