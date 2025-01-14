using BorrowingService.InventoryClient;

namespace BorrowingService.Features.Borrows.DTOs
{
	public class BookCountInLibraryDTO
	{
		public int BookId { get; set; }
		public LibraryDTO Library { get; set; }
		public int Count { get; set; }
	}
}
