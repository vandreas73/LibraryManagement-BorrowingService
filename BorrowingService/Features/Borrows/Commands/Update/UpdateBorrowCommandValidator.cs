﻿using BorrowingService.UserService;
using FluentValidation;
using static BorrowingService.UserService.UserManager;

namespace BorrowingService.Features.Borrows.Commands.Update
{
	public class UpdateBorrowCommandValidator : AbstractValidator<UpdateBorrowCommand>
	{
		private readonly CatalogClient.CatalogClient catalogClient;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly UserManagerClient userManagerClient;

		public UpdateBorrowCommandValidator(CatalogClient.CatalogClient catalogClient,
			InventoryClient.InventoryClient inventoryClient, UserManagerClient userManagerClient)
		{
			this.catalogClient = catalogClient;
			this.inventoryClient = inventoryClient;
			this.userManagerClient = userManagerClient;

			RuleFor(x => x.UserId)
				.NotEmpty()
				.MustAsync(async (id, cancellation) =>
				{
					var user = await userManagerClient.GetAsync(new UserIdRequest { Id = id });
					return user != null;
				})
						.WithMessage("User not found");

			RuleFor(x => x.LibraryId).NotEmpty()
						.MustAsync(LibraryExists)
						.WithMessage("Library does not exist");

			RuleFor(x => x.BookId).NotEmpty()
					.MustAsync(BookExists)
					.WithMessage("Book does not exist");

			RuleFor(x => x.BorrowedAt).NotEmpty();

			RuleFor(x => x.DueDate)
						.Must((borrow, dueDate) => dueDate >= borrow.BorrowedAt)
						.WithMessage("DueDate must not be earlier than BorrowedAt.");
		}

		private async Task<bool> LibraryExists(int libraryId, CancellationToken cancellationToken)
		{
			try
			{
				await inventoryClient.LibrariesGETAsync(libraryId);
				return true;
			}
			catch (InventoryClient.ApiException ex) when (ex.StatusCode == 404)
			{
				return false;
			}
		}

		private async Task<bool> BookExists(int bookId, CancellationToken cancellationToken)
		{
			try
			{
				await catalogClient.BooksGETAsync(bookId);
				return true;
			}
			catch (CatalogClient.ApiException ex) when (ex.StatusCode == 404)
			{
				return false;
			}
		}
	}
}
