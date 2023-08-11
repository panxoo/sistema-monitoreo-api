using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MonitorApi.Data
{
    public class UsersDbContext: IdentityDbContext 
    {

       public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options) { }

    }
}
