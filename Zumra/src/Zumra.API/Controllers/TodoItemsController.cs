using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zumra.API.Common;
using Zumra.Application.Features.TodoItems.Commands;
using Zumra.Application.Features.TodoItems.Queries;

namespace Zumra.API.Controllers
{
    [ApiController]
    [Route(ApiRoutes.TodoItems)]
    public class TodoItemsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TodoItemDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllTodoItems.Query());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetById(int id)
        {
            var result = await mediator.Send(new GetTodoItemById.Query { Id = id });

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTodoItem.Command command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTodoItem.Command command)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteTodoItem.Command { Id = id });

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}