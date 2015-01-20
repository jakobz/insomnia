using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia.Tests.Example.Models;

namespace Insomnia.Tests.Example.Context
{
    public class TestDbContext : DbContext, ITestDbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
