using BorrowingService.CatalogClient;
using BorrowingService.Exceptions;
using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.InventoryClient;
using BorrowingService.Models;
using BorrowingService.UserService;
using FluentValidation;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BorrowingService.Features.Borrows.Commands.Create
{
	public class CreateBorrowHandler : IRequestHandler<CreateBorrowCommand, Borrow>
	{
		private readonly BorrowContext context;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly UserManager.UserManagerClient userManagerClient;
		private readonly CatalogClient.CatalogClient catalogClient;

		public CreateBorrowHandler(BorrowContext context, CatalogClient.CatalogClient catalogClient,
			InventoryClient.InventoryClient inventoryClient, UserManager.UserManagerClient userManagerClient)
		{
			this.context = context;
			this.inventoryClient = inventoryClient;
			this.userManagerClient = userManagerClient;
			this.catalogClient = catalogClient;

		}
		public async Task<Borrow> Handle(CreateBorrowCommand request, CancellationToken cancellationToken)
		{
			var validator = new CreateBorrowCommandValidator(catalogClient, inventoryClient, userManagerClient);
			FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(request);

			if (!result.IsValid)
			{
				throw new BadRequestException(result.ToString("~"));
			}

			var libraryBook = await inventoryClient.BookAsync(request.LibraryId, request.BookId);
			var borrowedCount = await context.Borrows.Where(b => b.BookId == request.BookId && b.LibraryId == request.LibraryId && b.ReturnedAt == null).CountAsync();
			if (libraryBook.Count <= borrowedCount)
			{
				throw new BadRequestException("Book is not available in the library");
			}

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
