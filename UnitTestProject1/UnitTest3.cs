using System;
using System.Linq;
using System.Windows.Media.Converters;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UroborosApp.Model;
using UroborosApp.Pages;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest3
	{
		[TestMethod]
		public void AuthTestSuccess()
		{
			var page = new LoginPage();
			bool allTestsPassed = true;

			string failedUsers = "";

			using (var db = new UroborosDBEntities())
			{
				var users = db.users.ToList();

				foreach (var user in users)
				{
					try
					{
						string testPassword = "password_for_" + user.name;

						bool result = page.Auth(user.name, testPassword);

						if (!result)
						{
							allTestsPassed = false;
							failedUsers += $"{user.name}, ";
						}
					}
					catch (Exception ex)
					{
						allTestsPassed = false;
						failedUsers += $"{user.name} (error: {ex.Message}), ";
					}

					Assert.IsTrue(allTestsPassed, $"Авторизация не удалась для всех пользователей: {failedUsers.TrimEnd(',', ' ')}");
				}
			}
		}
	}
}
