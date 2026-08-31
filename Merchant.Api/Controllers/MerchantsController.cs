namespace Merchant.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MerchantsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet("get-all-merchant")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetMerchantQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{Guid}")]
    public async Task<IActionResult> Get([FromRoute] GetMerchantByGuidQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("business-type")]
    public async Task<IActionResult> GetByBusinessType([FromQuery] GetMerchantByBusinessTypeQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("create-merchant")]
    public async Task<IActionResult> Create([FromBody] CreateMerchantCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { Guid = result.Value.GuidId }, result.Value)
            : result.ToProblem();
    }
    [HttpPut("update-merchant")]
    public async Task<IActionResult> Update([FromBody] UpdateMerchantCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{Guid}")]
    public async Task<IActionResult> Remove([FromRoute] DeleteMerchantCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] MerchantSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] MerchantFilterQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
