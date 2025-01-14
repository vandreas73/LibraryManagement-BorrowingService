using BorrowingService.CatalogClient;
using BorrowingService.UserService;

namespace BorrowingService.Models
{
	public class Borrow
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		public int UserId { get; set; }
		public int LibraryId { get; set; }
		public DateOnly BorrowedAt { get; set; }
		public DateOnly DueDate { get; set; }
		public DateOnly? ReturnedAt { get; set; }

	}
}
