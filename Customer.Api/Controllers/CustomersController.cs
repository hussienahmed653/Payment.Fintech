namespace Customer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet("get-all-customer")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetCustomerQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{Guid}")]
    public async Task<IActionResult> Get([FromRoute] GetCustomerByGuidQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("business-type")]
    public async Task<IActionResult> GetByBusinessType([FromQuery] GetCustomerByBusinessTypeQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("create-customer")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { Guid = result.Value.GuidId }, result.Value)
            : result.ToProblem();
    }
    [HttpPut("update-customer")]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{Guid}")]
    public async Task<IActionResult> Remove([FromRoute] DeleteCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] CustomerSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] CustomerFilterQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
