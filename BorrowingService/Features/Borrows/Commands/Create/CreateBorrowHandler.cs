using BorrowingService.CatalogClient;
using BorrowingService.Exceptions;
using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.InventoryClient;
using BorrowingService.Models;
using BorrowingService.UserService;
using FluentValidation;
using Grpc.Core;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BorrowingService.Features.Borrows.Commands.Create
{
	public class CreateBorrowHandler : IRequestHandler<CreateBorrowCommand, Borrow>
	{
		private readonly BorrowContext context;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly UserManager.UserManagerClient userManagerClient;
		private readonly CatalogClient.CatalogClient catalogClient;
		private readonly CreateBorrowCommandValidator validator;

		public CreateBorrowHandler(BorrowContext context, CatalogClient.CatalogClient catalogClient,
			InventoryClient.InventoryClient inventoryClient, UserManager.UserManagerClient userManagerClient)
		{
			this.context = context;
			this.inventoryClient = inventoryClient;
			this.userManagerClient = userManagerClient;
			this.catalogClient = catalogClient;
			this.validator = new CreateBorrowCommandValidator(catalogClient, inventoryClient, userManagerClient);

		}
		public async Task<Borrow> Handle(CreateBorrowCommand request, CancellationToken cancellationToken)
		{
			FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(request);

			if (!result.IsValid)
			{
				throw new BadRequestException(result.ToString("~"));
			}

			//TODO: Check if the book is available

			var borrow = new Borrow
			{
				LibraryId = request.LibraryId,
				UserId = request.UserId,
				BookId = request.BookId,
				BorrowedAt = request.BorrowedAt,
				DueDate = request.DueDate,
				Id = 0,
				ReturnedAt = null
			};

			await context.AddAsync(borrow);

			await context.SaveChangesAsync();

			return borrow;
		}
	}
}
