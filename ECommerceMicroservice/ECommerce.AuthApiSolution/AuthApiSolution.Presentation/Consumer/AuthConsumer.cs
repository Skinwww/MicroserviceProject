using AuthApiSolution.Infrastructure.Data;
using AuthApiSolution.Presentation.Domain.Entities;
using MassTransit;

namespace AuthApiSolution.Presentation.Consumer
{
    public class AuthConsumer(AuthDbContext _context) : IConsumer<AppUser>
    {
        
            public async Task Consume(ConsumeContext<AppUser> context)
            {
                _context.Users.Add(context.Message);
                await _context.SaveChangesAsync();
            }
        
    }
}
