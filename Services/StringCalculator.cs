namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public string Add(string input)
        {
            if (input == "") return "0";

            var (customDelimiter, inputBody) = parseCustomDelimiter(input);

            // If a custom delimiter was supplied, add it to the delimiter array
            char[] delimiters = customDelimiter == null ? new[] { ',', '\n' } : new[] { ',', '\n', (char)customDelimiter };

            string normalizedInput = inputBody;
            foreach (char c in delimiters) normalizedInput = normalizedInput.Replace(c, ',');
            string[] inputArray = normalizedInput.Split(',');


            //Numbers outside the range of the decimal data type will be converted to zero.
            decimal[] decimalArray = inputArray.Select((s) => {
                    decimal val = 0;
                    decimal.TryParse(s, out  val);
                    return val > 1000 ? 0 : val;
                })
                .ToArray();

            AssertNoNegativeNumbers(decimalArray);

            return decimalArray.Sum().ToString();
        }

        private (char?, string) parseCustomDelimiter(string input)
        {
            char? delimiter = null;
            string body;

            // If the beginning of the input looks like the custom delimiter specifier
            if (input.Length > 4 && input.StartsWith("//") && input[3] == '\n')
            {
                // Parse the input into delimiter and body
                delimiter = input[2];
                body = input.Substring(4);
            } else
            {
                body = input;
            }
            return (delimiter, body);
        }

        private void AssertNoNegativeNumbers(decimal[] decimalArray)
        {
            decimal[] negativeNumbers = decimalArray.Where(d => d < 0).ToArray();
            if (negativeNumbers.Length > 0)
            {
                throw new FormatException($"{Config.GetErrorMessage("NegativeNumbers")} Input contained {string.Join(",", negativeNumbers)}");
            }
        }
    }

}
