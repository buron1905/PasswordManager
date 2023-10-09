using Services;
using System.Text.RegularExpressions;
using Services.Abstraction.Exceptions;
using static QRCoder.PayloadGenerator.SwissQrCode;
using Services.Auth;
using Moq;
using Services.Abstraction.Data;
using Models.DTOs;

namespace UnitTests
{
    [TestFixture]
    public class AuthServiceTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("nonexistent@testuser", false, false)]
        [TestCase("invalidToken@testuser", false, false, "invalidToken")]
        [TestCase("confirmed@testuser", true, true)]
        [TestCase("validToken@testuser", false, true)]
        public async Task AuthService_Test(string email, bool emailConfirmed, bool expectedResult, string token = "validToken")
        {
            // ARRANGE
            // Create a mock of the IUserService
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(email != "nonexistent@testuser" ? new UserDTO()
            {
                EmailAddress = email,
                EmailConfirmed = emailConfirmed,
                EmailConfirmationToken = "validToken"
            } : null);

            userService.Setup(x => x.UpdateAsync(It.IsAny<UserDTO>()))
            .ReturnsAsync(new UserDTO()
            {
                EmailAddress = email,
                EmailConfirmed = true,
                EmailConfirmationToken = "validToken"
            });

            var authService = new AuthService(userService.Object, null, null, null);

            // ACT
            var result = await authService.ConfirmEmailAsync(email, token);

            // ASSERT
            Assert.That(result, Is.EqualTo(expectedResult));
        }





        //[Test]
        //public async Task AuthService_Test()
        //{
        //    var userService = new Mock<IUserService>();
        //    userService.Setup(x => x.GetByEmailAsync("email@email")).ReturnsAsync(new UserDTO()
        //    {
        //        EmailAddress = "email@email",
        //        EmailConfirmed = false
        //    });

        //    userService.Setup(x => x.UpdateAsync(It.IsAny<UserDTO>()))
        //    .ReturnsAsync(new UserDTO()
        //    {
        //        EmailAddress = "email@email",
        //        EmailConfirmed = false
        //    });

        //    var authService = new AuthService(userService.Object, null, null, null);

        //    var result = await authService.ConfirmEmailAsync("email@email", "token");

        //    Assert.AreEqual(result, true);
        //}

    }
}