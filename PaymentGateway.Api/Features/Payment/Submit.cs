using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using MediatR;
using PaymentGateway.Api.Dto;
using PaymentGateway.ApplicationServices;
using PaymentGateway.Domain.Values;
using PaymentGateway.SharedKernel.Date;

namespace PaymentGateway.Api.Features.Payment
{
    public class Submit
    {
        public class Command : IRequest<PaymentRequestResponse>
        {
            public string        CardNumber { get; set; }
            public int           Ccv        { get; set; }
            public ExpiryDateDto ExpiryDate { get; set; }
            public MoneyDto      Money      { get; set; }
        }

        public class Handler : IRequestHandler<Command, PaymentRequestResponse>
        {
            private readonly IProcessPayment _processor;

            public Handler(IProcessPayment processor)
            {
                _processor = processor;
            }

            public async Task<PaymentRequestResponse> Handle(Command req, CancellationToken cancellationToken)
            {
                // In an asynchronous version, we would return directly a generated id
                // and let an queue handler create + process the payment.
                // We would then send the response through a callback to the caller
                var paymentId = await _processor.CreatePayment(req.CardNumber,
                                                               req.Ccv,
                                                               req.ExpiryDate.Month,
                                                               req.ExpiryDate.Year,
                                                               req.Money.Amount,
                                                               req.Money.Currency);

                var payment = await _processor.ProcessPayment(paymentId);

                return new PaymentRequestResponse
                {
                    Id     = payment.Id.IdAsString(),
                    Status = payment.Status.ToString()
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IProvideDateTime dateProvider)
            {
                RuleFor(t => t.CardNumber).Custom((s, context) =>
                {
                    var val = CardNumber.Validate(s);
                    PopulateContextFailuresFromValidationResult(val, context);
                });
                RuleFor(t => t.Ccv).Custom((s, context) =>
                {
                    var val = Ccv.Validate(s);
                    PopulateContextFailuresFromValidationResult(val, context);
                });
                RuleFor(command => command.ExpiryDate).Custom((s, context) =>
                {
                    var val = ExpiryDate.Validate(s.Month, s.Year, dateProvider);
                    PopulateContextFailuresFromValidationResult(val, context);
                });
                RuleFor(command => command.Money).Custom((s, context) =>
                {
                    var val = Money.Validate(s.Amount, s.Currency);
                    PopulateContextFailuresFromValidationResult(val, context);
                });
            }

            private static void PopulateContextFailuresFromValidationResult(ValidationResult val, CustomContext context)
            {
                if (val.IsValid)
                {
                    return;
                }

                foreach (var error in val.Errors)
                {
                    context.AddFailure(error.ErrorMessage);
                }
            }
        }
    }
}
