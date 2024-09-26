namespace StringCalculator.Services
{
    public class StringCalculator
    {
        public string Add(string input)
        {
            if (input == "") return "0";

            string[] inputArray = input.Split(',');
            decimal[] decimalArray = inputArray.Select((s) => {
                    decimal val = 0;
                    decimal.TryParse(s, out  val);
                    return val;
                })
                .ToArray();

            List<decimal> decimalList = new List<decimal>(decimalArray);

            return decimalList.Sum().ToString();
        }
    }
}
