namespace StringCalculator.Tests
{
    using StringCalculator.Common;
    using System.ComponentModel.DataAnnotations;

    public class ConfigTest
    {
        [Theory]
        [InlineData("MoreThanTwoNumbers", "Input can only contain two numbers.")]
        [InlineData("NegativeNumbers", "Input cannot negative numbers.")]
        public void ShouldReturnMessage(string messageKey, string expected) { 
            var config = new Config();
            var result = config.GetErrorMessage(messageKey);

            Assert.Equal(expected, result);
        }
    }
}
