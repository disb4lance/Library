using Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class UserFactoryTests
    {
        [Theory]
        [InlineData("adminuser", typeof(AdminUserFactory))]
        [InlineData("regularuser", typeof(RegularUserFactory))]
        public void UserFactory_ShouldReturnCorrectFactoryBasedOnUsername(string username, Type expectedFactoryType)
        {
            // Act
            IUserFactory userFactory;
            if (username.ToLower().Contains("admin"))
            {
                userFactory = new AdminUserFactory();
            }
            else
            {
                userFactory = new RegularUserFactory();
            }

            // Assert
            Assert.IsType(expectedFactoryType, userFactory);
        }
    }
}
