using Client.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class EmailValidatorTests
    {
        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("another.test@domain.org", true)]
        public void IsValidEmail_ShouldReturnCorrectValidationResult(string email, bool expected)
        {
            // Arrange
            var validator = new EmailValidator();

            // Act
            bool result = validator.IsValidEmail(email);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
