using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Values;
using PaymentGateway.Read.Repositories;

namespace PaymentGateway.Read.Projections
{
    public class PaymentApprovedHandler : INotificationHandler<PaymentApproved>
    {
        private readonly IPaymentRepository _repository;

        public PaymentApprovedHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PaymentApproved notification, CancellationToken cancellationToken)
        {
            var paymentView = await _repository.GetById(notification.AggregateId);
            paymentView.Status = PaymentStatus.Approved.ToString();

            await _repository.Save(paymentView);
        }
    }
}
