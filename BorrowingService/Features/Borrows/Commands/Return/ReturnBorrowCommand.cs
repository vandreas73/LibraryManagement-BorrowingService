using BorrowingService.Models;
using MediatR;

namespace BorrowingService.Features.Borrows.Commands.Return
{
	public record ReturnBorrowCommand(int Id) : IRequest<Borrow>;
}
