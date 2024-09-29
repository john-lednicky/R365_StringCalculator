namespace StringCalculator.Common
{
    /// <summary>
    /// Provides functionality to calculate the sum of numbers given as input.
    /// </summary>
    /// <remarks>
    /// This interface defines the contract for calculating the sum of numbers.
    /// Implementations must adhere to the rules and behavior defined in the StringCalculator class.
    /// </remarks>
    /// <seealso cref="StringCalculator.Services.StringCalculator"/>
    public interface IStringCalculator
    {
        string Add(string input, string delimiter = "\n", bool allowNegatives = false, decimal numberLimit = 1000);
    }
}
