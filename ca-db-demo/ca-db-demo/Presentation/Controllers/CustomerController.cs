
using Application.Users.Commands;
using Application.Users.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private IMediator _mediator;

    public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "")]
    public async Task<ActionResult<Customer>> AddUserAsync(CreateUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(command));
    }

    [HttpDelete(Name = "")]
    public async Task<ActionResult<Customer>> DeleteUserAsync(DeleteUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

            return Ok(await _mediator.Send(command));
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<Customer>> GetUserAsync(string id)
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(new GetUserQuery(id)));
    }

    [HttpPut(Name = "")]
    public async Task<ActionResult<Customer>> UpdateUserAsync(UpdateUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        try
        {
            return Ok(await _mediator.Send(command));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet(Name = "")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllUsersAsync()
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(new GetAllUserQuery()));
    }
}