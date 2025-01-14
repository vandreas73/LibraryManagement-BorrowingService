using BorrowingService.Features.Borrows.DTOs;
using BorrowingService.InventoryClient;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowingService.Features.Borrows.Queries.GetBookFreeCount
{
	public class GetBookFreeCountHandler : IRequestHandler<GetBookFreeCountQuery, ICollection<BookCountInLibraryDTO>>
	{
		private readonly BorrowContext context;
		private readonly InventoryClient.InventoryClient inventoryClient;

		public GetBookFreeCountHandler(BorrowContext context, InventoryClient.InventoryClient inventoryClient)
		{
			this.context = context;
			this.inventoryClient = inventoryClient;
		}

		public async Task<ICollection<BookCountInLibraryDTO>> Handle(GetBookFreeCountQuery request, CancellationToken cancellationToken)
		{
			var libraryBooks = await inventoryClient.LibrariesHavingBookAsync(request.BookId);
			if (request.LibraryId != null)
			{
				libraryBooks = libraryBooks.Where(l => l.Id == request.LibraryId).ToList();
			}
			var bookCountInLibraryDTOs = new List<BookCountInLibraryDTO>();

			foreach (var libraryBook in libraryBooks)
			{
				var borrowedCount = await context.Borrows.Where(b => b.BookId == request.BookId && b.LibraryId == libraryBook.LibraryId && b.ReturnedAt == null).CountAsync();
				bookCountInLibraryDTOs.Add(new BookCountInLibraryDTO
				{
					BookId = request.BookId,
					LibraryId = libraryBook.LibraryId,
					LibraryName = libraryBook.LibraryName,
					LibraryAddress = libraryBook.LibraryAddress,
					FreeCount = libraryBook.Count - borrowedCount
				});
			}

			return bookCountInLibraryDTOs;
		}
	}
}
