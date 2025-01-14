using BorrowingService.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BorrowingService.Features.Borrows.Queries.Get
{
	public class GetBorrowHandler : IRequestHandler<GetBorrowQuery, Borrow>
	{
		private readonly BorrowContext context;

		public GetBorrowHandler(BorrowContext context)
		{
			this.context = context;
		}
		public async Task<Borrow> Handle(GetBorrowQuery request, CancellationToken cancellationToken)
		{
			var borrow = await context.Borrows.FindAsync(request.Id);
			return borrow;
		}
	}
}
