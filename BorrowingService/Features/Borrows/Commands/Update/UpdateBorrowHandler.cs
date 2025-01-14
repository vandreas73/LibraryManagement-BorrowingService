using BorrowingService.Exceptions;
using BorrowingService.Models;
using BorrowingService.UserService;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BorrowingService.Features.Borrows.Commands.Update
{
	public class UpdateBorrowHandler : IRequestHandler<UpdateBorrowCommand, Borrow>
	{
		private readonly BorrowContext context;
		private readonly CatalogClient.CatalogClient catalogClient;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly UserManager.UserManagerClient userManagerClient;

		public UpdateBorrowHandler(BorrowContext context, CatalogClient.CatalogClient catalogClient,
			InventoryClient.InventoryClient inventoryClient, UserManager.UserManagerClient userManagerClient)
		{
			this.context = context;
			this.catalogClient = catalogClient;
			this.inventoryClient = inventoryClient;
			this.userManagerClient = userManagerClient;
		}
		public async Task<Borrow> Handle(UpdateBorrowCommand request, CancellationToken cancellationToken)
		{

			var validator = new UpdateBorrowCommandValidator(catalogClient, inventoryClient, userManagerClient);
			FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(request);

			if (!result.IsValid)
			{
				throw new BadRequestException(result.ToString("~"));
			}

			var borrow = await context.Borrows.FindAsync(request.Id);
			if (borrow == null)
			{
				throw new NotFoundException();
			}

			borrow.LibraryId = request.LibraryId;
			borrow.UserId = request.UserId;
			borrow.BookId = request.BookId;
			borrow.BorrowedAt = request.BorrowedAt;
			borrow.DueDate = request.DueDate;
			borrow.Id = request.Id;
			borrow.ReturnedAt = request.ReturnedAt;

			context.SaveChanges();

			return borrow;
		}
	}
}
