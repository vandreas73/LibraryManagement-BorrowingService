using BorrowingService.InventoryClient;

namespace BorrowingService.Features.Borrows.DTOs
{
	public class BookCountInLibraryDTO
	{
		public int BookId { get; set; }
		public int LibraryId { get; set; }
		public string LibraryName { get; set; }
		public string LibraryAddress { get; set; }
		public int FreeCount { get; set; }
	}
}
