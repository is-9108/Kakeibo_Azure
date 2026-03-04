using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using Kakeibo.Domain.Entities;
using Kakeibo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscription
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateSubscriptionsAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Add(new SubscriptionEntity
            {
                Name = request.Name,
                Amount = request.Amount,
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteSubscriptionsAsync(int Id, CancellationToken cancellationToken = default)
        {
            var subscription = _context.Subscriptions.Where(x => x.Id == Id).FirstOrDefault();
            if (subscription == null)
                throw new InvalidCastException("サブスクが存在しない");

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SubscriptionResponse>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
        {
            var subscriptions = await _context.Subscriptions.ToListAsync(cancellationToken);
            return subscriptions
                .Select(s => new SubscriptionResponse(s.Id,s.Name,s.Amount))
                .ToList();
        }

        public async Task UpdateSubscriptionsAsync(UpdateSubscriptionRequest request, CancellationToken cancellationToken = default)
        {
            var subscription = _context.Subscriptions.Where(s => s.Id == request.Id).FirstOrDefault();

            if (subscription == null)
                throw new InvalidCastException("サブスクが存在しない");

            subscription.Name = request.Name;
            subscription.Amount = request.Amount;

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
