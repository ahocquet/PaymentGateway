using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Values;
using PaymentGateway.Read.Models;
using PaymentGateway.Read.Repositories;

namespace PaymentGateway.Read.Projections
{
    public class PaymentRequestedHandler : INotificationHandler<PaymentRequested>
    {
        private readonly IPaymentRepository _repository;

        public PaymentRequestedHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(PaymentRequested notification, CancellationToken cancellationToken)
        {
            var paymentView = new PaymentView
            {
                Id         = notification.Id.IdAsString(),
                CardNumber = notification.CreditCard.CardNumber.MaskCardNumber(),
                Amount     = notification.Amount.Amount,
                Currency   = notification.Amount.Currency.ToString(),
                Status     = PaymentStatus.Pending.ToString()
            };

            return _repository.Save(paymentView);
        }
    }
}
