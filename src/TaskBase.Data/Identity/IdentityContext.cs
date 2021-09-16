using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Data.Utils;

namespace TaskBase.Data.Identity
{
    public class IdentityContext : IdentityDbContext<User>
    {
        readonly ConnectionStrings _connectionStrings;

        public IdentityContext(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionStrings.GetConnectionString("IdentityDb"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
