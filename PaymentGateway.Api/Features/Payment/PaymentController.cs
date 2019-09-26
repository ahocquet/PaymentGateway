using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Dto;

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

        //[HttpGet]
        //[Route("getById/{id}")]
        //public async Task<FraudAnalysisApplication> Get(Guid id)
        //{
        //    var result = await _mediator.Send(new Get.Query(id));
        //    return result;
        //}
    }
}
