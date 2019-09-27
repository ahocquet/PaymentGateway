using System.Threading.Tasks;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.ApplicationServices
{
    public interface IProcessPayment
    {
        Task<PaymentId> CreatePayment(string cardNumber, int ccv, int expiryMonthDate, int expiryYearDate, double amount, Currency currency);
        Task<Payment> ProcessPayment(PaymentId paymentId);
    }
}