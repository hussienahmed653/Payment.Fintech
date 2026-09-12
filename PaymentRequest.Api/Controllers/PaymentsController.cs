namespace PaymentRequest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet("{reference}")]
    public async Task<IActionResult> GetByReference([FromRoute] GetPaymentRequestByReferenceQuery query)
    { 
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
    [HttpPost("payments")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { Guid = result.Value.GuidId }, result.Value)
            : result.ToProblem();
    }
    [HttpPost("{reference}/pay")]
    public async Task<IActionResult> Pay([FromRoute] string reference, [FromHeader(Name ="X-Idempotency-Key")] string idemPotencyId)
    {
        var command = new PayPaymentCommand(reference, idemPotencyId);
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
