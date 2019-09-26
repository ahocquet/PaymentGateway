using System.Threading.Tasks;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.BankProvider.ACL
{
    public interface IAcquiringBank
    {
        Task<BankTransaction> ProcessPayment(Payment payment);
    }
}