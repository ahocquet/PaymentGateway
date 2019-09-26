using System;
using PaymentGateway.Domain.Values;

namespace PaymentGateway.Domain.Exceptions
{
    public class UnknownBankStatusException : Exception
    {
        public UnknownBankStatusException(string status, BankTransactionId transactionId, PaymentId paymentId)
            : base($"The transaction {transactionId} for the payment {paymentId} returned an unknown status: {status}")
        {
        }
    }
}
