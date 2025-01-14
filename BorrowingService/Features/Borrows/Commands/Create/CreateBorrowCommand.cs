using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Create
{
	public record CreateBorrowCommand(int BookId, int UserId, int LibraryId, DateOnly BorrowedAt, DateOnly DueDate) : IRequest<Borrow>;
}
