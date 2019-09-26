//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using FluentValidation;
//using MediatR;
//using PaymentGateway.Domain.Values;
//using PaymentGateway.EventSourcing.Core;

//namespace PaymentGateway.ApplicationServices.Features.FraudAnalysisApplication
//{
//    public class Get
//    {
//        public class Query : IRequest<Domain.Entities.FraudAnalysisApplication>
//        {
//            public Query(Guid id)
//            {
//                Id = id;
//            }

//            public Guid Id { get; set; }
//        }

//        public class Validator : AbstractValidator<Query>
//        {
//            public Validator()
//            {
//                RuleFor(t => t.Id).NotEmpty();
//            }
//        }

//        public class Handler : IRequestHandler<Query, Domain.Entities.FraudAnalysisApplication>
//        {
//            private readonly IRepository<Domain.Entities.FraudAnalysisApplication, FraudAnalysisId> _repository;

//            public Handler(IRepository<Domain.Entities.FraudAnalysisApplication, FraudAnalysisId> repository)
//            {
//                _repository = repository;
//            }

//            public async Task<Domain.Entities.FraudAnalysisApplication> Handle(Query request, CancellationToken cancellationToken)
//            {
//                var result = await _repository.Get(new FraudAnalysisId(request.Id));

//                return result;
//            }
//        }
//    }
//}
