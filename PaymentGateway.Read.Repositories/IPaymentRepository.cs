using System.Threading.Tasks;
using PaymentGateway.Domain.Values;
using PaymentGateway.Read.Models;

namespace PaymentGateway.Read.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaymentView> GetById(PaymentId id);
        Task Save(PaymentView payment);
    }
}