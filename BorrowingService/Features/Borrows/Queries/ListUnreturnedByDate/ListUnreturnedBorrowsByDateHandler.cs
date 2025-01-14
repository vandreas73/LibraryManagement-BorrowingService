using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.Models;
using BorrowingService.UserService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService.Features.Borrows.Queries.ListByDate
{
	public class ListUnreturnedBorrowsByDateHandler : IRequestHandler<ListUnreturnedBorrowsByDateQuery, ICollection<BorrowUserBookLibraryDTO>>
	{
		private readonly BorrowContext context;
		private readonly CatalogClient.CatalogClient catalogClient;
		private readonly InventoryClient.InventoryClient inventoryClient;
		private readonly UserManager.UserManagerClient userManagerClient;

		public ListUnreturnedBorrowsByDateHandler(BorrowContext context, CatalogClient.CatalogClient catalogClient,
			InventoryClient.InventoryClient inventoryClient, UserManager.UserManagerClient userManagerClient)
		{
			this.context = context;
			this.catalogClient = catalogClient;
			this.inventoryClient = inventoryClient;
			this.userManagerClient = userManagerClient;
		}
		public async Task<ICollection<BorrowUserBookLibraryDTO>> Handle(ListUnreturnedBorrowsByDateQuery request, CancellationToken cancellationToken)
		{
			IQueryable<Borrow> borrowQuery = borrowQuery = context.Borrows.Where(b => b.DueDate >= request.MinimumDueDate && b.ReturnedAt == null);
			if (request.LibraryId != null) { 
				borrowQuery = borrowQuery.Where(b => b.LibraryId == request.LibraryId);
			}
			var borrows = await borrowQuery.OrderBy(b => b.DueDate).ToListAsync();
			var borrowUserBookLibraryDtos = new List<BorrowUserBookLibraryDTO>();
			foreach (var borrow in borrows)
			{
				var library = await inventoryClient.LibrariesGETAsync(borrow.LibraryId);
				var book = await catalogClient.BooksGETAsync(borrow.BookId);
				var user = await userManagerClient.GetAsync(new UserIdRequest { Id = borrow.UserId });
				borrowUserBookLibraryDtos.Add(new BorrowUserBookLibraryDTO(
					borrow.Id,
					borrow.BookId,
					book.Title,
					book.AuthorName,
					borrow.UserId,
					user.Name,
					borrow.LibraryId,
					library.Name,
					borrow.BorrowedAt,
					borrow.DueDate,
					borrow.ReturnedAt));
			}
			return borrowUserBookLibraryDtos;
		}
	}
}
