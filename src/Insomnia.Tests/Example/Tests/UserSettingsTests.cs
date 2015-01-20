using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Insomnia.Tests.Example.Context;
using NUnit.Framework;
using EntityFramework.Extensions;
using Insomnia.Tests.Example.Models;
using Insomnia.Tests.Example.Controllers;
using Insomnia.Tests.Example.Infrastructure;
using Insomnia.Tests.Example.ViewModels;
using System.Data.Entity;

namespace Insomnia.Tests.Example.Tests
{
    [TestFixture]
    public class UserSettingsTests
    {
        public void WriteTestData()
        {
            using(TestDbContext db = new TestDbContext())
            {
                db.UserProfiles.Add(new UserProfile()
                {
                    TimeZoneId = "TZ1"
                });
                db.SaveChanges();
            }
        }

        void WrapTest<T>(Func<UserSettingsController, TestDbContext, T> action, Action<T, TestDbContext> check)
        {
            using (var scope = new TransactionScope())
            {
                WriteTestData();

                T result;

                using (var db = new TestDbContext())
                using (var controller = new UserSettingsController())
                {
                    result = action(controller, db);
                }

                using (var db = new TestDbContext())
                {
                    check(result, db);
                }
            }
        }

        [Test]
        public void ControllerCanGet()
        {
            WrapTest(
                (controller, db) => controller.Get(new FormRequest<UserProfileSettingsVM>()
                    {
                        ID = db.UserProfiles.First().ID
                }),
                (result, db) => 
                {
                    Is.Equals("TZ1", result.ViewState.TimeZoneID);
                }
            );
        }

        [Test]
        public void ControllerCanUpdate()
        {
            WrapTest(
                (controller, db) => controller.Save(new FormRequest<UserProfileSettingsVM>()
                {
                    ID = db.UserProfiles.First().ID,
                    ViewState = new UserProfileSettingsVM()
                    {
                        TimeZoneID = "TZ2"
                    }
                }),
                (IAsyncResult, db) => {
                    Is.Equals("TZ2", db.UserProfiles.First().TimeZoneId);
                }
            );
        }
    }
}
