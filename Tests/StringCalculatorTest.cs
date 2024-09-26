namespace StringCalculator.Tests
{
    using StringCalculator.Services;

    public class StringCalculatorTest
    {
        Messages messages = new();

        [Theory]
        [InlineData("", "0")]
        [InlineData("20", "20")]
        [InlineData("1,5000", "5001")]
        [InlineData("4,-3", "1")]
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

    }
}