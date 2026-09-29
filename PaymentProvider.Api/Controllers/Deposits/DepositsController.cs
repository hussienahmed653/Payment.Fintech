using PaymentProvider.Application.Deposit.Command;

namespace PaymentProvider.Api.Controllers.Deposits;

[Route("api/[controller]")]
[ApiController]
public class DepositsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpPost("")]
    public async Task<IActionResult> Deposit(InitiateDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result.Value);
    }
}
