using Kakeibo.Application.DTOs;
using Kakeibo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Services
{
    public class Subscription : ISubscription
    {
        private readonly ISubscription _subscription;

        public Subscription(ISubscription subscription)
        {
            _subscription = subscription;
        }

        public async Task CreateSubscriptionsAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default)
        {
            await _subscription.CreateSubscriptionsAsync(request, cancellationToken);
        }

        public async Task DeleteSubscriptionsAsync(int Id, CancellationToken cancellationToken = default)
        {
            await _subscription.DeleteSubscriptionsAsync(Id, cancellationToken);
        }

        public async Task<IReadOnlyList<SubscriptionResponse>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
        {
            return await _subscription.GetAllSubscriptionsAsync(cancellationToken);
        }

        public async Task UpdateSubscriptionsAsync(UpdateSubscriptionRequest request, CancellationToken cancellationToken = default)
        {
            await _subscription.UpdateSubscriptionsAsync(request, cancellationToken);
        }
    }
}
