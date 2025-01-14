using BorrowingService.CatalogClient;
using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.InventoryClient;
using BorrowingService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService.Features.Borrows.Queries.ListByUser
{
	public class ListBorrowsByUserHandler : IRequestHandler<ListBorrowsByUserQuery, List<BorrowBookLibraryDto>>
	{
		private readonly BorrowContext context;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly CatalogClient.CatalogClient catalogClient;

		public ListBorrowsByUserHandler(BorrowContext context, CatalogClient.CatalogClient catalogClient, InventoryClient.InventoryClient inventoryClient)
		{
			this.context = context;
			this.inventoryClient = inventoryClient;
			this.catalogClient = catalogClient;
		}
		public async Task<List<BorrowBookLibraryDto>> Handle(ListBorrowsByUserQuery request, CancellationToken cancellationToken)
		{
			var borrows = await context.Borrows.Where(b => b.UserId == request.userId).ToListAsync();
			var borrowBookLibraryDtos = new List<BorrowBookLibraryDto>();
			foreach (var borrow in borrows)
			{
				var library = await inventoryClient.LibrariesGETAsync(borrow.LibraryId);
				var book = await catalogClient.BooksGETAsync(borrow.BookId);
				borrowBookLibraryDtos.Add(new BorrowBookLibraryDto(
					borrow.Id,
					borrow.BookId,
					book.Title,
					book.AuthorName,
					borrow.UserId,
					borrow.LibraryId,
					library.Name,
					borrow.BorrowedAt,
					borrow.DueDate,
					borrow.ReturnedAt));
			}
			return borrowBookLibraryDtos;
		}
	}
}
