using Kakeibo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakeibo.Application.Interfaces
{
    public interface ISubscription
    {
        Task<IReadOnlyList<SubscriptionResponse>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default);
        Task CreateSubscriptionsAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default);
        Task UpdateSubscriptionsAsync(UpdateSubscriptionRequest request, CancellationToken cancellationToken = default);
        Task DeleteSubscriptionsAsync(int Id, CancellationToken cancellationToken = default);
    }
}
