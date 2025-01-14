using BorrowingService.Exceptions;
using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Delete
{
	public class DeleteBorrowHandler : IRequestHandler<DeleteBorrowCommand>
	{
		private readonly BorrowContext context;

		public DeleteBorrowHandler(BorrowContext context)
		{
			this.context = context;
		}
		public async Task Handle(DeleteBorrowCommand request, CancellationToken cancellationToken)
		{
			var borrow = await context.Borrows.FindAsync(request.Id);
			if (borrow == null)
			{
				throw new NotFoundException();
			}
			context.Borrows.Remove(borrow);
			await context.SaveChangesAsync();
		}
	}
}
