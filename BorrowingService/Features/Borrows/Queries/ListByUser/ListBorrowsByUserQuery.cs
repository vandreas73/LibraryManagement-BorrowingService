using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Queries.ListByUser
{
	public record ListBorrowsByUserQuery(int userId) : IRequest<List<BorrowBookLibraryDto>>;
}
