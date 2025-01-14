using BorrowingService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService.Features.Borrows.Queries.List
{
	public class ListBorrowsHandler : IRequestHandler<ListBorrowsQuery, List<Borrow>>
	{
		private readonly BorrowContext context;

		public ListBorrowsHandler(BorrowContext context)
		{
			this.context = context;
		}
		public async Task<List<Borrow>> Handle(ListBorrowsQuery request, CancellationToken cancellationToken)
		{
			return await context.Borrows.ToListAsync();
		}
	}
}
