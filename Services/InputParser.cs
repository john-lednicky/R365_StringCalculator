namespace StringCalculator.Common
{
    public class InputParser(IConfig config) : IInputParser
    {

        public (string[], string) ExtractDelimitersAndBody(string input)
        {

            string[] delimiters = [];
            string body;

            if (input.StartsWith("//["))
            {
                int prefixBoundary = input.IndexOf("]\n") + 1; // The newline separating the prefix from the body.

                if (prefixBoundary == 0) throw new FormatException(config.GetErrorMessage("CustomDelimiterMalformed"));

                string delimiterSpec = input.Substring(2, prefixBoundary - 2); //The delimiters without the leading slashes.

                body = input.Substring(prefixBoundary + 1);
                delimiters = parseDelimiterSpec(delimiterSpec);
            }
            else if (input.StartsWith("//"))
            {
                // A single character delimiter has fixed positions for the value and the end-of-prefix delimiter.
                int firstNewlinePos = input.IndexOf('\n');
                if (firstNewlinePos != 3) throw new FormatException(config.GetErrorMessage("CustomDelimiterMalformed"));
                delimiters = [input[2].ToString()];
                body = input.Substring(firstNewlinePos + 1);
            }
            else
            {
                // There is no prefix, just return the entire input as the body.
                body = input;
            }
            return (delimiters, body);
        }

        private string[] parseDelimiterSpec(string delimiterSpec)
        {
            //Remove outer brackets
            var spec = delimiterSpec.Substring(1, delimiterSpec.Length - 2);

            //Split on inner brackets
            var returnValue = spec.Split("][");

            //Remove empties and return
            return returnValue.Where(s => s != string.Empty).ToArray();
        }

    }
}
