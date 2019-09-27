using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Dto;
using PaymentGateway.Read.Models;

namespace PaymentGateway.Api.Features.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PaymentRequestResponse), (int) HttpStatusCode.OK)]
        public async Task<PaymentRequestResponse> Submit(Submit.Command command)
            => await _mediator.Send(command);

        [HttpGet]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PaymentView), (int) HttpStatusCode.OK)]
        [Route("{Id}")]
        public async Task<PaymentView> Get([FromRoute] Get.Query command)
            => await _mediator.Send(command);
    }
}
