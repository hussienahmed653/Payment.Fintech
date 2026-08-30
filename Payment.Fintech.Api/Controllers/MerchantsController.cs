using Payment.Fintech.Application.Merchant.Command.DeleteMerchant;
using Payment.Fintech.Application.Merchant.Command.UpdateMerchant;
using Payment.Fintech.Application.Merchant.Query.Filter;
using Payment.Fintech.Application.Merchant.Query.Search;

namespace Payment.Fintech.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MerchantsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetMerchantQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("{Guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid Guid)
    {
        var query = new GetMerchantByGuidQuery(Guid);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("business-type")]
    public async Task<IActionResult> GetByBusinessType([FromQuery] string businessType)
    {
        var query = new GetMerchantByBusinessTypeQuery(businessType);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("CreateMerchant")]
    public async Task<IActionResult> Create([FromBody] MerchantRequest request)
    {
        var query = new CreateMerchantCommand(request);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new {Guid = result.Value.GuidId}, result.Value)
            : result.ToProblem();
    }
    [HttpPut("{Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid Guid, [FromBody] UpdateMerchantRequest request)
    {
        var query = new UpdateMerchantCommand(Guid, request);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{Guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid Guid)
    {
        var query = new DeleteMerchantCommand(Guid);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string search)
    {
        var query = new MerchantSearchQuery(search);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] MerchantFilterRequest request)
    {
        var query = new MerchantFilterQuery(request);
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
