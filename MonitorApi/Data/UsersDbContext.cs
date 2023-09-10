using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonitorApi.Models.DataBase;

namespace MonitorApi.Data
{
    public class UsersDbContext: IdentityDbContext<User>
    {

       public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options) { }

        public DbSet<RefreshToken> refreshTokens { get; set; }

    }
}
