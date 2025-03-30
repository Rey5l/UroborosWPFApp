using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UroborosApp.Model;
using UroborosApp.Pages;

namespace UnitTestProject1
{
	[TestClass]
	public class RegistrationTests
	{
		private RegisterPage _registerPage;
		private string _testUser;
		private string _testEmail;

		[TestInitialize]
		public void Setup()
		{
			_registerPage = new RegisterPage();
			_testUser = "User1";
			_testEmail = "user1@gmail.com";

			CleanupTestData(_testUser, _testEmail);
		}

        [TestCleanup]
        public void Cleanup()
        {
            CleanupTestData(_testUser, _testEmail);
        }

        private void CleanupTestData(string username, string email)
        {
            using (var db = new UroborosDBEntities())
            {
                var user = db.users.FirstOrDefault(u => u.name == username || u.email == email);
                if (user != null)
                {
                    db.users.Remove(user);
                    db.SaveChanges();
                }
            }
        }

        // TC_Reg_1 - Регистрация с корректными данными
        [TestMethod]
        public void Register_WithValidData_ReturnsSuccess()
        {
            var result = _registerPage.RegisterUser(
                _testUser,
                _testEmail,
                "ValidPass123",
                "ValidPass123");

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Вы успешно зарегистрировались!", result.Message);
        }

        // TC_Reg_2 - Регистрация с существующим логином
        [TestMethod]
        public void Register_WithExistingUsername_ReturnsError()
        {
            _registerPage.RegisterUser(_testUser, _testEmail, "ValidPass123", "ValidPass123");

            var result = _registerPage.RegisterUser(
                _testUser,
                "different@email.com",
                "NewPass123",
                "NewPass123");

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Пользователь с таким логином уже существует", result.Message);
        }

        // TC_Reg_3 - Регистрация с некорректным email
        [TestMethod]
        public void Register_WithInvalidEmail_ReturnsError()
        {
            var result = _registerPage.RegisterUser(
                _testUser,
                "invalid-email",
                "ValidPass123",
                "ValidPass123");

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Email введен некорректно", result.Message);
        }

        // TC_Reg_4 - Регистрация с коротким паролем (<8 символов)
        [TestMethod]
        public void Register_WithShortPassword_ReturnsError()
        {
            var result = _registerPage.RegisterUser(
                _testUser,
                _testEmail,
                "passwor",
                "passwor");

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Длина пароля должна быть больше 8 символов", result.Message);
        }

        // TC_Reg_5 - Регистрация с пустыми полями
        [TestMethod]
        public void Register_WithEmptyFields_ReturnsError()
        {
            var result = _registerPage.RegisterUser("", "", "", "");

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Все поля должны быть заполнены", result.Message);
        }

        // TC_Reg_6 - Регистрация с специальными символами
        [TestMethod]
        public void Register_WithSpecialCharacters_ReturnsSuccess()
        {
            var result = _registerPage.RegisterUser(
                "user$123!",
                "special$@example.com",
                "P@ssw0rd!",
                "P@ssw0rd!");

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Вы успешно зарегистрировались!", result.Message);

            CleanupTestData("user$123!", "special$@example.com");
        }

        // TC_Reg_7 - Регистрация с пробелами
        [TestMethod]
        public void Register_WithSpaces_ReturnsError()
        {
            var result = _registerPage.RegisterUser(
                "user name",
                "test user@example.com",
                "pass word",
                "pass word");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("не должно содержать пробелы") ||
                         result.Message.Contains("Email введен некорректно"));
        }

        // TC_Reg_8 - Регистрация с длинными данными
        [TestMethod]
        public void Register_WithLongData_ReturnsError()
        {
            var longString = new string('a', 2000);

            var result = _registerPage.RegisterUser(
                longString,
                $"{longString}@example.com",
                longString,
                longString);

            Assert.IsFalse(result.IsSuccess);
        }
    }
}
