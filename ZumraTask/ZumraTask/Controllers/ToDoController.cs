using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZumraTask.Application.Commands;
using ZumraTask.Application.Queries;

namespace ZumraTask.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IMediator _mediator;
    public ToDoController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetAllToDoItemsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) =>
        Ok(await _mediator.Send(new GetToDoItemByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToDoItemCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateToDoItemCommand cmd)
    {
        if (id != cmd.Id) return BadRequest();
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteToDoItemCommand(id));
        return NoContent();
    }
}
