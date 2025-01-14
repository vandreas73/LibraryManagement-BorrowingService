using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.ListByDate
{
	public record ListUnreturnedBorrowsByDateQuery(DateOnly MinimumDueDate, int? LibraryId) : IRequest<ICollection<BorrowUserBookLibraryDTO>>;
}
