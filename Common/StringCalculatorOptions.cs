namespace StringCalculator.Common
{
    public class StringCalculatorOptions { 
        public string Input { get; }
        public string Delimiter { get; }
        public bool AllowNegatives { get; }
        public int NumberLimit { get; }

        public StringCalculatorOptions(string input, string delimiter = "\n", bool allowNegatives = false, int numberLimit = 1000)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Delimiter = delimiter;
            AllowNegatives = allowNegatives;
            NumberLimit = numberLimit;
        }
    }
}
