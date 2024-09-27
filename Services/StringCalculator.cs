namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public string Add(string input)
        {
            if (input == "") return "0";

            var (customDelimiter, inputBody) = parseCustomDelimiter(input);

            // If a custom delimiter was supplied, add it to the delimiter array
            string[] delimiters = customDelimiter == null ? [ ",", "\n" ] : [ ",", "\n", customDelimiter];

            string normalizedInput = inputBody;

            foreach (string c in delimiters) normalizedInput = normalizedInput.Replace(c, ",");
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

        private (string?, string) parseCustomDelimiter(string input)
        {
            string? delimiter = null;
            string body;
            int firstNewlinePos = input.IndexOf('\n');

            // If the beginning of the input looks like the custom delimiter specifier
            if (input.StartsWith("//") && firstNewlinePos >= 3)
            {
                string delimiterSpec = input.Substring(2, firstNewlinePos - 2);
                
                if (delimiterSpec.Length == 1)
                {
                    // If the delimiter is a single character, return it
                    delimiter = delimiterSpec[0].ToString();
                }
                else {
                    //Otherwise, check that the delimiter is well formed with open and closed braces, then return it.
                    if (delimiterSpec.StartsWith("[") && delimiterSpec.EndsWith("]"))
                    {
                        delimiter = delimiterSpec.Substring(1, delimiterSpec.Length - 2);
                    }
                    else {
                        throw new FormatException(Config.GetErrorMessage("CustomDelimiterMalformed"));
                    }
                }
                body = input.Substring(firstNewlinePos + 1);
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
