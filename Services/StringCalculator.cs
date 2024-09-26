namespace StringCalculator.Services
{
    public class StringCalculator
    {
        Messages messages = new Messages();

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
                    return val;
                })
                .ToArray();

            AssertNoNegativeNumbers(decimalArray);

            List<decimal> decimalList = new List<decimal>(decimalArray);

            return decimalList.Sum().ToString();
        }

        private void AssertNoNegativeNumbers(decimal[] decimalArray)
        {
            decimal[] negativeNumbers = decimalArray.Where(d => d < 0).ToArray();
            if (negativeNumbers.Length > 0)
            {
                throw new FormatException($"{messages["NegativeNumbers"]} Input contained {string.Join(",", negativeNumbers)}");
            }
        }
    }

}
