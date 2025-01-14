namespace BorrowingService.Features.Borrows.DTOs
{
	public record BorrowUserBookLibraryDTO(int Id, int BookId, string BookTitle, string BookAuthor, int UserId, string UserName, int LibraryId, string LibraryName, DateOnly BorrowedAt, DateOnly DueDate, DateOnly? ReturnedAt);
}
