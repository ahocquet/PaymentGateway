using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Values;
using PaymentGateway.Read.Repositories;

namespace PaymentGateway.Read.Projections
{
    public class PaymentRejectedHandler : INotificationHandler<PaymentRejected>
    {
        private readonly IPaymentRepository _repository;

        public PaymentRejectedHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PaymentRejected notification, CancellationToken cancellationToken)
        {
            var paymentView = await _repository.GetById(notification.AggregateId);
            paymentView.Status = PaymentStatus.Rejected.ToString();

            await _repository.Save(paymentView);
        }
    }
}
