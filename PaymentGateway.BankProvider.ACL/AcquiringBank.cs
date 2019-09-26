using System;
using System.Threading.Tasks;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.BankProvider.ACL
{
    public class AcquiringBank : IAcquiringBank
    {
        private readonly Random _random;

        public AcquiringBank()
        {
            _random = new Random();
        }

        public async Task<BankTransaction> ProcessPayment(Payment payment)
        {
            var status        = _random.Next(100) <= 50 ? BankTransactionStatus.Success : BankTransactionStatus.Failed;
            var transactionId = new BankTransactionId(Guid.NewGuid());
            var result        = BankTransaction.Create(transactionId, status, payment.Id);

            // Simulate http latency
            await Task.Delay(TimeSpan.FromSeconds(_random.Next(3)));

            return result;
        }
    }
}
