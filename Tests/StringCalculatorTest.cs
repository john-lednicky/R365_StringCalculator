namespace StringCalculator.Tests
{
    using StringCalculator.Services;
    using StringCalculator.Common;

    public class StringCalculatorTest
    {
        private readonly IConfig config;
        private readonly IInputParser inputParser ;
        private readonly IValidators validators;

        public StringCalculatorTest() {
            config = new Config();
            inputParser = new InputParser(config);
            validators = new Validators(config);
        }

        [Theory]
        [InlineData("", "0")]
        [InlineData("20", "20")]
        [InlineData("1,500", "501")]
        [InlineData("1.1,2.2", "3.3")]
        public void ShouldAddTwoNumbers_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("1,2,3", "6")]
        [InlineData("1,2,3,4,5,6,7,8,9,10,11,12", "78")]
        public void ShouldAddMoreThanTwoNumbers_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("1\n500", "501")]
        [InlineData("1\n2,3", "6")]
        [InlineData("1,2\n3", "6")]
        public void ShouldAllowNewlineDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("-1,2", "-1")]
        [InlineData("-1,2,-3,-4", "-1,-3,-4")]
        public void ShouldDisallowNegativeNumbers(string input, string negatives)
        {
            var calculator = new StringCalculator(inputParser, validators);

            Assert.Throws<FormatException>(
                () =>
                {
                    try
                    {
                        calculator.Add(input);
                    }
                    catch (FormatException ex)
                    {
                        Assert.StartsWith(config.GetErrorMessage("NegativeNumbers"), ex.Message);
                        Assert.EndsWith(negatives, ex.Message);
                        throw;
                    }
                }
            );
        }

        [Theory]
        [InlineData("2,1001,6", "8")]
        public void ShouldDiscardNumbersOver1000(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("2,1001,6,", "8")]
        public void ShouldNotCareAboutTrailingDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("//#\n2#5", "7")]
        [InlineData("//~\n2~5,3", "10")]
        [InlineData("//~\n2~5\n3", "10")]
        [InlineData("//,\n2,ff,100", "102")]
        public void ShouldAllowSingleCharacterCustomDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("//[***]\n11***22***33", "66")]
        [InlineData("//[***]\n11,22***33", "66")]
        [InlineData("//[***]\n11***22,33", "66")]
        [InlineData("//[*,*]\n11*,*22,33", "66")]
        [InlineData("//[*,*]\n11*,*22\n33", "66")]
        public void ShouldAllowStringCustomDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("//***]\n11***22,33")]
        [InlineData("//[*,*\n11*,*22,33")]
        [InlineData("//[*,*][$#\n11*,*22,33")]
        public void ShouldDisallowMalformedPrefix(string input)
        {
            var calculator = new StringCalculator(inputParser, validators);

            Assert.Throws<FormatException>(
                () =>
                {
                    try
                    {
                        calculator.Add(input);
                    }
                    catch (FormatException ex)
                    {
                        Assert.StartsWith(config.GetErrorMessage("CustomDelimiterMalformed"), ex.Message);
                        throw;
                    }
                }
            );
        }

        [Theory]
        [InlineData("//[*][!!][r9r]\n11r9r22*hh*33!!44", "110")]
        [InlineData("//[***][---]\n11***22,33---44", "110")]
        [InlineData("//[***][-[-]\n11***22-[-33,44", "110")]
        [InlineData("//[***][-,-]\n11***22-,-33,44", "110")]
        [InlineData("//[***][-\n-]\n11***22-\n-33,44", "110")]
        public void ShouldAllowMultipleStringCharacterCustomDelimiters(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("", "0 = 0")]
        [InlineData("20", "20 = 20")]
        [InlineData("1,500", "1+500 = 501")]
        [InlineData("1.1,2.2", "1.1+2.2 = 3.3")]
        public void ShouldAddTwoNumbersAndShowCalculation_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,2,3", "1+2+3 = 6")]
        [InlineData("1,2,3,4,5,6,7,8,9,10,11,12", "1+2+3+4+5+6+7+8+9+10+11+12 = 78")]
        public void ShouldAddMoreThanTwoNumbersAndShowCalculation_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "^^", "0")]
        [InlineData("1^^500", "^^", "501")]
        [InlineData("1.1^^2.2", "^^", "3.3")]
        public void ShouldUseParameterizedDelimiter(string input, string delimiter, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input, delimiter: delimiter);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("", true, "0")]
        [InlineData("-1,500", true, "499")]
        [InlineData("1.1,2.2",true, "3.3")]
        public void ShouldUseParameterizedNegatives(string input, bool allowNegatives, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input, allowNegatives: allowNegatives);
            Assert.EndsWith(expected, result);
        }

        [Theory]
        [InlineData("2,1001,6", 2000, "1009")]
        [InlineData("2,2001,6", 2000, "8")]
        public void ShouldAllowNumbersOver1000(string input, decimal numberLimit, string expected)
        {
            var calculator = new StringCalculator(inputParser, validators);
            var result = calculator.Add(input, numberLimit: numberLimit);
            Assert.EndsWith(expected, result);
        }
    }
}