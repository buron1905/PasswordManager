using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace UITests
{
    [TestFixture]
    public class TestBase
    {
        protected IWebDriver Driver { get; private set; }
        protected IDictionary<string, object> Vars { get; private set; }
        protected IJavaScriptExecutor _js;
        public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public void Initialize()
        {
            Driver = new ChromeDriver();
            _js = (IJavaScriptExecutor)Driver;
            Vars = new Dictionary<string, object>();

            Driver.Navigate().GoToUrl(TestContext.Parameters["BaseUrl"]);
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }

    }
}
