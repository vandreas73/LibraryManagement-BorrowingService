using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Delete
{
	public record DeleteBorrowCommand(int Id) : IRequest;
}
