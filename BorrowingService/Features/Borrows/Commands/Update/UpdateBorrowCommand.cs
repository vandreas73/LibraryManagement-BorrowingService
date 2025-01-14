using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Update
{
	public record UpdateBorrowCommand(int Id, int BookId, int UserId, int LibraryId, DateOnly BorrowedAt, DateOnly DueDate, DateOnly? ReturnedAt) : IRequest<Borrow>;
}
