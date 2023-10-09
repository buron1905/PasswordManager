using OpenQA.Selenium;

namespace UITests
{
    [TestFixture]
    public class UITests : TestWithAuthBase
    {

        [Test]
        public void Password_AddPasswordTest()
        {
            // ARRANGE
            var passwordName = $"Password-{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss")}";
            var userName = "username";
            var url = "google.com";
            var notes = "Note";

            // ACT
            Driver.FindElement(By.LinkText("Add new")).Click();

            Driver.FindElement(By.Id("passwordName")).SendKeys(passwordName);
            Driver.FindElement(By.Id("floatingInputGroup2")).SendKeys(userName);
            Driver.FindElement(By.CssSelector(".bi-arrow-clockwise")).Click();
            Driver.FindElement(By.CssSelector(".btn-primary:nth-child(2)")).Click();
            Driver.FindElement(By.Id("url")).SendKeys(url);
            Driver.FindElement(By.Id("notes")).SendKeys(notes);
            Driver.FindElement(By.CssSelector(".btn-primary")).Click();
            Driver.FindElement(By.Id("searchInput")).SendKeys(passwordName);

            // ASSERT
            var passwordNameElement = By.XPath($"//*[contains(text(), '{passwordName}')]");
            var userNameElement = By.XPath($"//*[contains(text(), '{userName}')]");
            Assert.NotNull(Driver.FindElement(passwordNameElement));
            Assert.NotNull(Driver.FindElement(userNameElement));

            //Assert.That(Driver.FindElement(By.CssSelector(".pointer:nth-child(2)")).Text, Is.EqualTo(passwordName));
            //Assert.That(Driver.FindElement(By.CssSelector(".pointer:nth-child(3)")).Text, Is.EqualTo(userName));
        }
    }
}