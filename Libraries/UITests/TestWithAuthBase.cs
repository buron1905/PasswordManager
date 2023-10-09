using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace UITests
{
    [TestFixture]
    public class TestWithAuthBase : TestBase
    {
        [SetUp]
        public void Login()
        {
            Driver.Navigate().GoToUrl(TestContext.Parameters["BaseUrl"] + "login");
            Thread.Sleep(500);
            Driver.FindElement(By.Id("emailAddress")).SendKeys(TestContext.Parameters["TestUserName"]);
            Driver.FindElement(By.Id("password")).SendKeys(TestContext.Parameters["TestUserPassword"]);
            Driver.FindElement(By.CssSelector(".btn-primary:nth-child(1)")).Click();

            Assert.That(Driver.FindElement(By.CssSelector(".toast-message")).Text, Is.EqualTo("Login successful"));
        }

    }
}
