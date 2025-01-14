using BorrowingService.Features.Borrows.Commands.Create;
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

		//[HttpGet]
		//public async Task<ActionResult<IEnumerable<Borrow>>> Get()
		//{
		//}

		//[HttpGet("{id}")]
		//public async Task<ActionResult<Borrow>> Get(int id)
		//{
		//}

		[HttpPost]
		public async Task<ActionResult<Borrow>> CreateBorrow(CreateBorrowCommand command)
		{
			var borrow = await mediator.Send(command);
			return CreatedAtAction("CreateBorrow", new { id = borrow.Id }, borrow);
		}


	}
}
