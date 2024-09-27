namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public interface IStringCalculator
        {
            string Add(string input, string delimiter = "\n", bool allowNegatives = false, decimal numberLimit = 1000);
        }
        /// <summary>
        /// Calculates the sum of numbers in a given string input using specified delimiters.
        /// </summary>
        /// <param name="input">
        /// The input string can take one of three forms:
        /// 
        /// 1. Simple form: Contains only numbers separated by default delimiters.
        ///   Example: "1\n2\n3"
        /// 
        /// 2. Single-character delimiter form: Starts with "//" followed by a single character then a newline
        ///   Example: "//,\n1,2,3"
        /// 
        /// 3. Multi-character delimiter form: Starts with "//" followed by a list of delimiters enclosed in square brackets, then a newline.
        ///   Example: "//[|][^^]\n1|2^^3,4,5"
        /// 
        /// The method will attempt to parse the input based on these formats, extracting custom delimiters if present.
        /// </param>
        /// <param name="delimiter">
        ///     Optional. Default is "\n". Specifies the delimiter between numbers.
        /// </param>
        /// <param name="allowNegatives">
        ///     Optional. Default is false, which will throw an exception if a negative number is supplied. When true, allows negative numbers.
        /// </param>
        /// <param name="numberLimit">
        ///     Optional. Default is 1000. Maximum allowed number value. Numbers exceeding this limit are treated as 0.
        /// </param>
        /// <returns>A formatted string showing the calculation and result, e.g., "1+2+3 = 6"</returns>
        /// <exception cref="FormatException">
        ///     Thrown when the input contains malformed custom delimiters or invalid characters.
        /// </exception>
        public string Add(string input, string delimiter = "\n", bool allowNegatives = false, decimal numberLimit = 1000)
        {
            if (input == "") return "0 = 0";

            var (customDelimiters, inputBody) = extractDelimitersAndBody(input);

            // If custom delimiters were supplied, add it to the delimiter array
            string[] delimiters = customDelimiters.Concat([ ",", delimiter]).ToArray();

            var inputArray = inputBody.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            //Numbers outside the range of the decimal data type will be converted to zero.
            decimal[] decimalArray = inputArray.Select((s) => {
                    decimal val = 0;
                    decimal.TryParse(s, out  val);
                    return val > numberLimit ? 0 : val;
                })
                .ToArray();

            if(!allowNegatives) AssertNoNegativeNumbers(decimalArray);

            string calculation = string.Join("+", decimalArray);
            string answer = decimalArray.Sum().ToString();
            return $"{calculation} = {answer}";
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
