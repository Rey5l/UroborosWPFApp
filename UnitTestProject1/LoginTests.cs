using System;
using System.Windows.Controls;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UroborosApp;
using UroborosApp.Pages;

namespace UnitTestProject1
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new LoginPage();
            Assert.IsTrue(page.Auth("Reysl", "password_for_Reysl"));
            Assert.IsFalse(page.Auth("user1", "12345"));
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth(" ", " "));
        }
    }
}
