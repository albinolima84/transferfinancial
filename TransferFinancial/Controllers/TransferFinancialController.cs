using Application.Command.Commands;
using Application.Command.Dtos;
using Application.Command.Responses;
using Application.Query.Requests;
using Application.Query.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferFinancialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferFinancialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(StatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] string transactionId)
        {
            var query = new StatusQuery(transactionId);

            var response = await _mediator.Send(query);

            if(response.IsFailure)
                return BadRequest(response.Messages);

            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RequestTransferResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] RequestTransferDto request)
        {
            if(request is null)
            {
                return BadRequest("Requisição inválida.");
            }

            var command = RequestTransferCommand.Create(request.AccountOrigin, request.AccountDestination, request.Value);
            if(command.IsFailure)
            {
                return BadRequest(command.Messages);
            }

            var response = await _mediator.Send(command.Value);

            if(response.IsFailure)
            {
                return BadRequest(response.Messages);
            }

            return Ok(response.Value);
        }
    }
}
