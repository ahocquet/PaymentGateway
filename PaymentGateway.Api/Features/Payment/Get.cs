using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PaymentGateway.Domain.Values;
using PaymentGateway.Read.Models;
using PaymentGateway.Read.Repositories;

namespace PaymentGateway.Api.Features.Payment
{
    public class Get
    {
        public class Query : IRequest<PaymentView>
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(t => t.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, PaymentView>
        {
            private readonly IPaymentRepository _repository;

            public Handler(IPaymentRepository repository)
            {
                _repository = repository;
            }

            public Task<PaymentView> Handle(Query query, CancellationToken cancellationToken)
                => _repository.GetById(new PaymentId(query.Id));
        }
    }
}
