using System;
using System.Threading.Tasks;
using PaymentGateway.BankProvider.ACL;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Values;
using PaymentGateway.DomainServices;
using PaymentGateway.EventSourcing.Core;
using PaymentGateway.SharedKernel.Date;

namespace PaymentGateway.ApplicationServices
{
    public class PaymentProcessor : IProcessPayment
    {
        private readonly IAggregateRepository<Payment, PaymentId> _paymentRepository;
        private readonly IPaymentService                 _paymentService;
        private readonly IAcquiringBank                  _bank;
        private readonly IProvideDateTime                _dateProvider;

        public PaymentProcessor(IAggregateRepository<Payment, PaymentId> paymentRepository,
                                IPaymentService paymentService,
                                IAcquiringBank bank,
                                IProvideDateTime dateProvider)
        {
            _paymentRepository = paymentRepository;
            _paymentService    = paymentService;
            _bank              = bank;
            _dateProvider      = dateProvider;
        }

        public async Task<PaymentId> CreatePayment(string cardNumber, int ccv, int expiryMonthDate, int expiryYearDate, double amount, Currency currency)
        {
            var card = CreditCard.Create(CardNumber.Create(cardNumber),
                                         ExpiryDate.Create(expiryMonthDate, expiryYearDate, _dateProvider),
                                         Ccv.Create(ccv));
            var money   = Money.Create(amount, currency);
            var payment = new Payment(PaymentId.Create(), card, money);

            await _paymentRepository.Save(payment);

            return payment.Id;
        }

        public async Task<Payment> ProcessPayment(PaymentId paymentId)
        {
            if (paymentId == null)
            {
                throw new ArgumentNullException(nameof(paymentId));
            }

            var payment = await _paymentRepository.Get(paymentId);
            if (_paymentService.HasAlreadyBeenProcessed(payment))
            {
                throw new PaymentAlreadyProcessedException(paymentId);
            }

            var transaction = await _bank.ProcessPayment(payment);
            _paymentService.UpdatePayment(payment, transaction);

            await _paymentRepository.Save(payment);
            return payment;
        }
    }
}
