using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.List
{
	public class ListBorrowsQuery : IRequest<List<Borrow>>
	{
	}
}
