using System;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.DomainServices
{
    public class PaymentService : IPaymentService
    {
        public bool HasAlreadyBeenProcessed(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            return payment.Status != PaymentStatus.Pending;
        }

        public void UpdatePayment(Payment payment, BankTransaction transaction)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            switch (transaction.Status)
            {
                case BankTransactionStatus.Failed:
                    payment.Reject(transaction.Id);
                    break;
                case BankTransactionStatus.Success:
                    payment.Approve(transaction.Id);
                    break;
                default:
                    throw new UnknownBankStatusException(transaction.Status.ToString(), transaction.Id, transaction.PaymentId);
            }
        }
    }
}
