using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.Get
{
	public record GetBorrowQuery(int Id) : IRequest<Borrow>;
}
