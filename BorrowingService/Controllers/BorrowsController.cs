using BorrowingService.Features.Borrows.Commands.Create;
using BorrowingService.Features.Borrows.Commands.Delete;
using BorrowingService.Features.Borrows.Commands.Update;
using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.Features.Borrows.Queries.Get;
using BorrowingService.Features.Borrows.Queries.List;
using BorrowingService.Features.Borrows.Queries.ListByUser;
using BorrowingService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BorrowsController : ControllerBase
	{
		private readonly IMediator mediator;

		public BorrowsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Borrow>>> GetBorrows()
		{
			var borrows = await mediator.Send(new ListBorrowsQuery());
			return Ok(borrows);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Borrow>> GetBorrow(int id)
		{
			var borrow = await mediator.Send(new GetBorrowQuery(id));
			return Ok(borrow);
		}

		[HttpPost]
		public async Task<ActionResult<Borrow>> CreateBorrow(CreateBorrowCommand command)
		{
			var borrow = await mediator.Send(command);
			return CreatedAtAction("CreateBorrow", new { id = borrow.Id }, borrow);
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<BorrowBookLibraryDto>> GetBorrowsByUser(int userId)
		{
			var borrows = await mediator.Send(new ListBorrowsByUserQuery(userId));
			return Ok(borrows);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await mediator.Send(new DeleteBorrowCommand(id));
			return NoContent();
		}

		[HttpPut]
		public async Task<ActionResult<Borrow>> UpdateBorrow(UpdateBorrowCommand command)
		{
			var borrow = await mediator.Send(command);
			return NoContent();
		}

	}
}
