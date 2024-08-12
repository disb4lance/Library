using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Classes;
namespace Tests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void HashPassword_ShouldReturnHashedValue()
        {
            // Arrange
            var hasher = new PasswordHasher();
            string password = "TestPassword123";

            // Act
            string hashedPassword = hasher.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }
    }
}
