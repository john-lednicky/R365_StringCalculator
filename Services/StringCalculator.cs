namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public string Add(string input)
        {
            if (input == "") return "0";

            char[] delimiters = { ',', '\n' };
            string normalizedInput = input;
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
