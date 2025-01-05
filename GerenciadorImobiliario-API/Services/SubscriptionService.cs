using GerenciadorImobiliario_API.Data;
using GerenciadorImobiliario_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorImobiliario_API.Services
{
    public class SubscriptionService
    {
        private readonly AppDbContext _context;

        public SubscriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserSubscription?> GetUserSubscriptionAsync(long userId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.SubscriptionPlan)
                .FirstOrDefaultAsync(us => us.UserId == userId && us.IsActive);
        }
    }
}
