using AuthApiSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApiSolution.Infrastructure.Data
{
    public class AuthDbContext: DbContext

    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
    }
}
