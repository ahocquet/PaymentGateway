using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Features.Infrastructure
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InfrastructureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InfrastructureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task SetupInfrastructure(Create.Command command)
            => await _mediator.Send(command);
    }
}
