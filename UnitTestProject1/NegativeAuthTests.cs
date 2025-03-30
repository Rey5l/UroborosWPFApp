using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UroborosApp.Pages;

namespace UnitTestProject1
{
	[TestClass]
	public class NegativeAuthTests
	{
		private LoginPage _loginPage;

		[TestInitialize]
		public void Setup()
		{
			_loginPage = new LoginPage();
		}

        // TC_Auth_2 - Неверный пароль
        [TestMethod]
        public void Auth_WithInvalidPassword_ReturnsFalse()
        {
            var result = _loginPage.Auth("User1", "wrong_password");

            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой при неверном пароле");
        }

        // TC_Auth_3 - Неверный логин
        [TestMethod]
        public void Auth_WithInvalidLogin_ReturnsFalse()
        {
            var result = _loginPage.Auth("nonexistent_user", "any_password");

            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой при неверном логине");
        }

        // TC_Auth_4 - Пустые поля
        [TestMethod]
        public void Auth_WithEmptyFields_ReturnsFalse()
        {
            var result = _loginPage.Auth("", "");

            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой при пустых полях");
        }

        // TC_Auth_6 - Данные с пробелами
        [TestMethod]
        public void Auth_WithSpacesInCredentials_ReturnsFalse()
        {
            var result = _loginPage.Auth("user with spaces", "pass with spaces");

            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой при наличии пробелов");
        }

        // TC_Auth_7 - Данные превышающие длину
        [TestMethod]
        public void Auth_WithTooLongCredentials_ReturnsFalse()
        {
            string longString = new string('a', 2000);

            var result = _loginPage.Auth(longString, longString);

            Assert.IsFalse(result, "Авторизация должна завершиться ошибкой при слишком длинных данных");
        }
    }
}
