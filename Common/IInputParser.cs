namespace StringCalculator.Common
{
    public interface IInputParser
    {
        (string[], string) ExtractDelimitersAndBody(string input);
    }
}
