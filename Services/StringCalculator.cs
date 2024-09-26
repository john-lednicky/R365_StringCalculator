namespace StringCalculator.Services
{
    public class StringCalculator
    {
        Messages messages = new Messages();

        public string Add(string input)
        {
            if (input == "") return "0";

            string[] inputArray = input.Split(',');

            //Numbers outside the range of the decimal data type will be converted to zero.
            decimal[] decimalArray = inputArray.Select((s) => {
                    decimal val = 0;
                    decimal.TryParse(s, out  val);
                    return val;
                })
                .ToArray();

            if (decimalArray.Length > 2) throw new FormatException(messages["MoreThanTwoNumbers"]);

            List<decimal> decimalList = new List<decimal>(decimalArray);

            return decimalList.Sum().ToString();
        }
    }
}
