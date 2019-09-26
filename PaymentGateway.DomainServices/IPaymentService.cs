using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.DomainServices
{
    public interface IPaymentService
    {
        bool HasAlreadyBeenProcessed(Payment payment);
        void UpdatePayment(Payment payment, BankTransaction transaction);
    }
}