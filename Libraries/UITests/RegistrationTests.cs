using OpenQA.Selenium;

namespace UITests
{
    [TestFixture]
    [Category("Registration")]
    public class RegistrationTests : TestBase
    {
        [SetUp]
        public void GoToRegistrationPage()
        {
            Driver.Navigate().GoToUrl(TestContext.Parameters["BaseUrl"] + "register");
        }

        [Test]
        public void Registration_ExistingUserTest()
        {
            // ACT
            FillRegistrationForm(TestContext.Parameters["TestUserName"]!, TestContext.Parameters["TestUserPassword"]!, TestContext.Parameters["TestUserPassword"]!);

            // ASSERT
            Assert.That(Driver.FindElement(By.Id("submitHelpBlock")).Text, Is.EqualTo("Error: Email is already used by another account."));
        }

        [TestCase("newuitestusernewuitestusernewuitestnewuitestusernewuitestusernewuitestnewuitestusernewuitestusernewuitestnewuitestusernewuitestusernewuitest_")]
        [TestCase("newuitestuser_")]
        public void Registration_SuccessfulRegistrationTest(string emailPrefix)
        {
            // ARRANGE
            var newValidEmail = $"{emailPrefix}{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss")}@testuser";
            var password = "testuser2023"; // 12 characters (minimum for password)

            // ACT
            FillRegistrationForm(newValidEmail, password, password);
            Thread.Sleep(1000);

            // ASSERT
            Assert.That(Driver.FindElement(By.CssSelector(".toast-message")).Text, Is.EqualTo("Registration successful"));
        }

        [TestCase("invalid_email", "ValidPassword123", "ValidPassword123", "Email is not in valid format.")]
        [TestCase("invalid_email", "invalid", "invalid2", "Email is not in valid format.", "Password must be at least 12 characters long.", "Passwords do not match.")]
        [TestCase("valid-email@example.com", "s", "s", null, "Password must be at least 12 characters long.")]
        [TestCase("valid-email@example.com", "short07", "short07", null, "Password must be at least 12 characters long.")]
        [TestCase("valid-email@example.com", "short", "short", null, "Password must be at least 12 characters long.")]
        [TestCase("valid-email@example.com", "short", "short2", null, "Password must be at least 12 characters long.", "Passwords do not match.")]
        [TestCase("valid-email@example.com", "ValidPassword123", "invalid", null, null, "Passwords do not match.")]
        [TestCase("valid-email@example.com", "ValidPassword123", "OtherValidPassword123", null, null, "Passwords do not match.")]
        [TestCase("", "", "", "This field is required.", "This field is required.", "This field is required.")]
        public void Registration_InvalidRegistrationTest(string email, string password, string confirmPassword,
            string? expectedErrorMessageField1 = null, string? expectedErrorMessageField2 = null, string? expectedErrorMessageField3 = null)
        {
            // ACT
            FillRegistrationForm(email, password, confirmPassword);

            // ASSERT
            var errorMessage1 = By.XPath($"//*[contains(text(), '{expectedErrorMessageField1}')]");
            var errorMessage2 = By.XPath($"//*[contains(text(), '{expectedErrorMessageField2}')]");
            var errorMessage3 = By.XPath($"//*[contains(text(), '{expectedErrorMessageField3}')]");

            if (expectedErrorMessageField1 != null)
                Assert.NotNull(Driver.FindElement(errorMessage1));
            if (expectedErrorMessageField2 != null)
                Assert.NotNull(Driver.FindElement(errorMessage2));
            if (expectedErrorMessageField3 != null)
                Assert.NotNull(Driver.FindElement(errorMessage3));
        }

        private void FillRegistrationForm(string email, string password, string confirmPassword)
        {
            Driver.FindElement(By.Id("emailAddress")).SendKeys(email);
            Driver.FindElement(By.Id("password")).SendKeys(password);
            Driver.FindElement(By.Id("confirmPassword")).SendKeys(confirmPassword);
            Driver.FindElement(By.CssSelector(".btn-primary:nth-child(1)")).Click();
        }
    }
}