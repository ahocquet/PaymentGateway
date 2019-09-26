using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Cosmos.Table;

namespace PaymentGateway.Api.Features.Infrastructure
{
    public class Create
    {
        public class Command : IRequest
        {
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly IDictionary<string, CloudTable> _tables;

            public Handler(IDictionary<string, CloudTable> tables)
            {
                _tables = tables;
            }

            protected override Task Handle(Command request, CancellationToken cancellationToken)
            {
                var tasks = _tables.Values
                                   .Select(table => table.CreateIfNotExistsAsync(cancellationToken))
                                   .ToArray();
                return Task.WhenAll(tasks);
            }
        }
    }
}
