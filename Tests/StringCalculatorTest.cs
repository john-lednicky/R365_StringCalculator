namespace StringCalculator.Tests
{
    using StringCalculator.Services;

    public class StringCalculatorTest
    {

        [Theory]
        [InlineData("", "0")]
        [InlineData("20", "20")]
        [InlineData("1,500", "501")]
        [InlineData("1.1,2.2", "3.3")]
        public void ShouldAddTwoNumbers_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,2,3", "6")]
        [InlineData("1,2,3,4,5,6,7,8,9,10,11,12", "78")]
        public void ShouldAddMoreThanTwoNumbers_HappyPath(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1\n500", "501")]
        [InlineData("1\n2,3", "6")]
        [InlineData("1,2\n3", "6")]
        public void ShouldAllowNewlineDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("-1,2", "-1")]
        [InlineData("-1,2,-3,-4", "-1,-3,-4")]
        public void ShouldDisallowNegativeNumbers(string input, string negatives)
        {
            var calculator = new StringCalculator();

            Assert.Throws<FormatException>(
                () =>
                {
                    try
                    {
                        calculator.Add(input);
                    }
                    catch (FormatException ex)
                    {
                        Assert.StartsWith(Config.GetErrorMessage("NegativeNumbers"), ex.Message);
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
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2,1001,6,", "8")]
        public void ShouldNotCareAboutTrailingDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("//#\n2#5", "7")]
        [InlineData("//~\n2~5,3", "10")]
        [InlineData("//~\n2~5\n3", "10")]
        [InlineData("//,\n2,ff,100", "102")]
        public void ShouldAllowSingleCharacterCustomDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("//[***]\n11***22***33", "66")]
        [InlineData("//[***]\n11,22***33", "66")]
        [InlineData("//[***]\n11***22,33", "66")]
        [InlineData("//[*,*]\n11*,*22,33", "66")]
        public void ShouldAllowMutlipleCharacterCustomDelimiter(string input, string expected)
        {
            var calculator = new StringCalculator();
            var result = calculator.Add(input);
            Assert.Equal(expected, result);
        }
    }
}