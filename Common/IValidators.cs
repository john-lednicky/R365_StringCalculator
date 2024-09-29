namespace StringCalculator.Common
{
    public interface IValidators
    {
        void AssertNoNegativeNumbers(decimal[] decimalArray);
    }
}
