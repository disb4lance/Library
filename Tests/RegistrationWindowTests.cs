using Classes.Models;
using Client;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    //public class RegistrationWindowTests
    //{
    //    [Fact]
    //    public async Task RegisterUserAsync_ShouldShowSuccessMessage_WhenRegistrationSucceeds()
    //    {
    //        // Arrange
    //        var httpClientMock = new Mock<HttpClient>();
    //        httpClientMock
    //            .Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
    //            .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

    //        var registrationWindow =  httpClientMock.Object;

    //        var user = new User { Email = "test@example.com", UserName = "testuser", PasswordHash = "hashedpassword" };

    //        // Act
    //        await registrationWindow.RegisterUserAsync(user);

    //        // Assert
    //        // Здесь можно проверить, было ли вызвано определенное сообщение или выполнены другие действия
    //    }
    //}
}
