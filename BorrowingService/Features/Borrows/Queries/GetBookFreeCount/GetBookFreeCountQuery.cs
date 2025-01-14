using BorrowingService.Features.Borrows.DTOs;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.GetBookFreeCount
{
	public record GetBookFreeCountQuery(int BookId, int? LibraryId) : IRequest<ICollection<BookCountInLibraryDTO>>;
}
