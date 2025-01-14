using BorrowingService.Exceptions;
using BorrowingService.Features.Borrows.Commands.Return;
using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Return
{
	public class ReturnBorrowHandler : IRequestHandler<ReturnBorrowCommand, Borrow>
	{
		private readonly BorrowContext context;

		public ReturnBorrowHandler(BorrowContext context)
		{
			this.context = context;
		}
		public async Task<Borrow> Handle(ReturnBorrowCommand request, CancellationToken cancellationToken)
		{
			var borrow = await context.Borrows.FindAsync(request.Id);
			if (borrow == null)
			{
				throw new NotFoundException("Borrow not found");
			}

			borrow.ReturnedAt = System.DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
			await context.SaveChangesAsync();
			return borrow;
		}
	}
}
