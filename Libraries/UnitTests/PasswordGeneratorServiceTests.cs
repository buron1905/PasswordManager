using Services;
using System.Text.RegularExpressions;
using Services.Abstraction.Exceptions;

namespace UnitTests
{
    [TestFixture]
    public class PasswordGeneratorServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0, false, false, false, false)]

        [TestCase(1, true, false, false, false)]
        [TestCase(1, false, true, false, false)]
        [TestCase(1, false, false, true, false)]
        [TestCase(1, false, false, false, true)]

        [TestCase(8, true, true, true, true)]

        [TestCase(8, true, true, true, false)]
        [TestCase(8, true, true, false, true)]
        [TestCase(8, true, false, true, true)]
        [TestCase(8, false, true, true, true)]

        [TestCase(8, true, true, false, false)]
        [TestCase(8, true, false, true, false)]
        [TestCase(8, true, false, false, true)]
        [TestCase(8, false, true, true, false)]
        [TestCase(8, false, true, false, true)]
        [TestCase(8, false, false, true, true)]

        [TestCase(8, true, false, false, false)]
        [TestCase(8, false, true, false, false)]
        [TestCase(8, false, false, true, false)]
        [TestCase(8, false, false, false, true)]
        public void GeneratePassword_BasicValidOptionsTest(int length, bool useNumbers, bool useSpecialChars, bool useUppercase, bool useLowercase)
        {
            var generatedPassword = PasswordGeneratorService.GeneratePassword(length, useNumbers, useSpecialChars, useUppercase, useLowercase);
            Assert.IsNotNull(generatedPassword);
            Assert.That(generatedPassword.Length, Is.EqualTo(length));

            if (useNumbers)
                Assert.IsTrue(generatedPassword.Any(char.IsDigit));
            else
                Assert.IsFalse(generatedPassword.Any(char.IsDigit));

            if (useUppercase)
                Assert.IsTrue(generatedPassword.Any(char.IsUpper));
            else
                Assert.IsFalse(generatedPassword.Any(char.IsUpper));

            if (useLowercase)
                Assert.IsTrue(generatedPassword.Any(char.IsLower));
            else
                Assert.IsFalse(generatedPassword.Any(char.IsLower));

            string strRegex = @"[!@#$%^&*]";
            Regex re = new Regex(strRegex);
            if (useSpecialChars)
                Assert.IsTrue(re.IsMatch(generatedPassword));
            else if (!useSpecialChars)
                Assert.IsFalse(re.IsMatch(generatedPassword));
        }

        // Options for 2
        [TestCase(1, true, true, false, false)]
        [TestCase(1, true, false, true, false)]
        [TestCase(1, true, false, false, true)]
        [TestCase(1, false, true, true, false)]
        [TestCase(1, false, true, false, true)]
        [TestCase(1, false, false, true, true)]

        [TestCase(1, true, true, true, false)]
        [TestCase(1, true, true, false, true)]
        [TestCase(1, true, false, true, true)]
        [TestCase(1, false, true, true, true)]
        [TestCase(1, true, true, true, true)]

        [TestCase(2, true, true, true, false)]
        [TestCase(2, true, true, false, true)]
        [TestCase(2, true, false, true, true)]
        [TestCase(2, false, true, true, true)]
        [TestCase(2, true, true, true, true)]

        [TestCase(3, true, true, true, true)]
        public void GeneratePassword_InvalidOptionsToLengthTest(int length, bool useNumbers, bool useSpecialChars, bool useUppercase, bool useLowercase)
        {
            Assert.Throws<PasswordGeneratorException>(() => PasswordGeneratorService.GeneratePassword(length, useNumbers, useSpecialChars, useUppercase, useLowercase));
        }

        [TestCase(-1, true, true, true, true)]
        [TestCase(-1, false, false, false, false)]
        [TestCase(0, true, true, true, true)]
        [TestCase(0, false, false, false, false)]
        [TestCase(1, false, false, false, false)]
        [TestCase(8, false, false, false, false)]
        public void GeneratePassword_ZeroLengthTest(int length, bool useNumbers, bool useSpecialChars, bool useUppercase, bool useLowercase)
        {
            var generatedPassword = PasswordGeneratorService.GeneratePassword(length, useNumbers, useSpecialChars, useUppercase, useLowercase);
            Assert.IsNotNull(generatedPassword);
            Assert.That(generatedPassword.Length, Is.EqualTo(0));
        }
    }
}