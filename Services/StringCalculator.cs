namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public string Add(string input)
        {
            if (input == "") return "0";

            var (customDelimiters, inputBody) = extractDelimitersAndBody(input);

            // If custom delimiters were supplied, add it to the delimiter array
            string[] delimiters = customDelimiters.Concat([ ",", "\n"]).ToArray();

            string[] inputArray = inputBody.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

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

        private (string[], string) extractDelimitersAndBody(string input)
        {
            string[] delimiters = [];
            string body;

            if (input.StartsWith("//["))
            {
                int prefixBoundary = input.IndexOf("]\n") + 1; // The newline separating the prefix from the body.

                if (prefixBoundary == 0) throw new FormatException(Config.GetErrorMessage("CustomDelimiterMalformed"));

                string delimiterSpec = input.Substring(2,prefixBoundary-2); //The delimiters without the leading slashes.

                body = input.Substring(prefixBoundary+1);
                delimiters = parseDelimiterSpec(delimiterSpec);
            }
            else if(input.StartsWith("//"))
            {
                // A single character delimiter has fixed positions for the value and the end-of-prefix delimiter.
                int firstNewlinePos = input.IndexOf('\n');
                if (firstNewlinePos != 3) throw new FormatException(Config.GetErrorMessage("CustomDelimiterMalformed"));
                delimiters = [ input[2].ToString() ];
                body = input.Substring(firstNewlinePos + 1);
            }
            else
            {
                // There is no prefix, just return the entire input as the body.
                body = input;
            }
            return (delimiters, body);
        }

        private string[] parseDelimiterSpec(string delimiterSpec) {
            //Remove outer brackets
            var spec = delimiterSpec.Substring(1,delimiterSpec.Length - 2);

            //Split on inner brackets
            var returnValue = spec.Split("][");

            //Remove empties and return
            return returnValue.Where(s => s != string.Empty).ToArray();
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
