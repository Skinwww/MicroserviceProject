using AuthApiSolution.Presentation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApiSolution.Infrastructure.Data
{
    public class AuthDbContext: DbContext

    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
    }
}
