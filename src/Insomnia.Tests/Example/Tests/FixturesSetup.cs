using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia.Tests.Example.Context;
using NUnit.Framework;

namespace Insomnia.Tests.Example.Tests
{
    [SetUpFixture]
    class FixturesSetup
    {
        [SetUp]
        public void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestDbContext>());
            using (TestDbContext db = new TestDbContext())
            {
                db.Database.Initialize(false);
            }
        }
    }
}
